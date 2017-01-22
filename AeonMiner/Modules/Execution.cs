using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;
using System.Diagnostics;

namespace AeonMiner.Modules
{
    using Data;
    using Enums;
    using Handlers;

    public sealed partial class BaseModule : Helpers
    {
        private Statistics stats;

        private State state = State.Check;
        private NodeHandle node;
        private bool isMoving;

        /// <summary>
        /// Initialize runtime.
        /// </summary>
        private bool Initialize()
        {
            if (mineTask.Name == string.Empty)
            {
                Host.Log("Please create or select a task.");
                return false;
            }

            if (!gps.Load(mineTask.MiningZone))
                return false;


            state = State.Check;

            if ((stats == null) || (stats.ZoneName != mineTask.MiningZone) || UI.IsResetStats())
            {
                stats = new Statistics(UI);
                stats.ZoneName = mineTask.MiningZone;
                stats.LaborStartedWith = Host.me.laborPoints;
            }

            // Events
            HookEvents();
            
            // Start tasks
            Task.Run(() => RunTimer(), token);

            return true;
        }

        /// <summary>
        /// Loop execution task.
        /// </summary>
        private void Loop()
        {
            while (token.IsAlive())
            {
                try
                {
                    Execute();
                }
                catch (StopException e)
                {
                    Host.Log(e.Message);
                    StopRequest();

                    return;
                }
                catch (Exception e)
                {
                    Host.Log(e.Message + " " + e.StackTrace);

                    // Default state
                    SetState(State.Check);
                }


                Utils.Delay(50, token);
            }
        }

        private void StopRequest()
        {
            Stop();

            if (settings.RunPlugin && settings.PluginRunName != string.Empty)
            {
                Host.RunPlugin(settings.PluginRunName);
            }
        }


        private void Execute()
        {
            /// ... Combat Check Here!

            switch (state)
            {
                case State.Check:
#if DEBUG
                    Host.Log("State: Check");
#endif
                    Check();
                    break;

                case State.Search:
#if DEBUG
                    Host.Log("State: Search");
#endif
                    Search();
                    break;

                case State.Move:
#if DEBUG
                    Host.Log("State: Move");
#endif
                    Move();
                    break;

                case State.Mine:
#if DEBUG
                    Host.Log("State: Mine");
#endif
                    Mine();
                    break;
            }
        }

        
        private void Check()
        {
            if (!InNavMesh(Host.me))
            {

            }

            SetState(State.Search);
        }

        private void Search()
        {
            if ((node = mining.GetNode()) == null)
            {
                if (TryPatrolToPoint())
                {
                    // Patrol delay
                    Utils.Delay(2450, 4250, token);
                }

                return;
            }

            if (!mining.IsNodeCheck())
                return;


            SetState(State.Move);
        }

        private void Move()
        {
            if (node.Dist() < 2)
            {
                SetState(State.Mine);
                return;
            }


            isMoving = true;

            Task.Run(() => MovingWatch(node.Get()), token);


            bool result = mining.MoveToNode();
            isMoving = false;

            if (!token.IsAlive())
                return;


            if (result)
            {
                SetState(State.Mine);

                Utils.Delay(50, 100, token);
            }
            else
            {
                Log(Host.GetLastError().ToString());
                SetState(State.Check);
            }
        }

        private void Mine()
        {
            if (!IsQuestTakeLocked() && node.IsFortunaVein())
            {
                TakeMiningQuest();
            }


            bool result = mining.MineVein();

            if (!token.IsAlive())
                return;


            if (result)
            {
                stats.VeinsMined++;

                if (node.IsFortunaVein(true))
                {
                    stats.FortunaVeins += 1;
                }
                else if (node.IsUnidentifiedVein(true))
                {
                    stats.UnidentifiedVeins += 1;
                }


                if (settings.AutoLevelUp && CanUpgradeLevel(13))
                {
                    UpgradeLevel(13);
                }

                Utils.Delay(650, 850, token);
            }

            
            SetState(State.Check);
        }


        private void RunTimer()
        {
            while (token.IsAlive())
            {
                stats.RunTime += 1;
                stats.AvgMinedPerHour = (int)((double)stats.VeinsMined / (double)stats.RunTime * 3600);

                // Estimate to burn labor
                var estimate = stats.GetBurnEstimate(Host.me.laborPoints);
                var esDateTime = new DateTime(estimate.Ticks);

                UI.UpdateLabel(UI.lbl_EstimatingTime, esDateTime.ToString("HH:mm:ss"));


                Utils.Delay(1000, token);
            }
        }

        private void PatrolWatch()
        {
            while (isMoving && token.IsAlive())
            {
                try
                {
                    if (mining.GetNode() != null)
                    {
                        Host.CancelMoveTo();
                        break;
                    }
                }
                catch
                {
                }

                Utils.Delay(50, token);
            }
        }

        private void MovingWatch(double x, double y, double z, bool isNavMesh = true)
        {
            Func<double> GetDist = () 
                => (isNavMesh) ? GetNavDist(x, y, z) : Host.dist(x, y, z);

            // Starting distance
            double beginDist = GetDist();


            while (isMoving && token.IsAlive())
            {
                try
                {
                    if (IsFreerunEnabled())
                    {
                        TryFreerun();
                    }

                    if (isNavMesh && IsWarpingEnabled())
                    {
                        TryWarping(x, y, z);
                    }

                    if (beginDist >= 16)
                    {
                        if (settings.UseDash)
                        {
                            if (Host.dist(x, y, z) > 7.5) // Increment dist by boosts used.
                            {
                                TryDashing();
                            }
                            else
                            {
                                CancelAnyBuffs(Buffs.Dash);
                            }
                        }

                        if (IsQuickstepEnabled() && Host.dist(x, y, z) > 6.5)
                        {
                            TryQuickstep();
                        }
                    }
                }
                catch
                {
                }


                Utils.Delay(50, token);
            }

            CancelBoosts();
        }


        private bool ComeInsideMesh()
        {
            
        }

        private bool TryPatrolToPoint()
        {
            if (!AnyPatrolPoints())
                return false;

            var point = GetPatrolPoints().OrderByDescending(p => GetNavDist(p.x, p.y, p.z)).First();


            isMoving = true;

            Task.Run(() => PatrolWatch(), token);
            Task.Run(() => MovingWatch(point.x, point.y, point.z), token);


            Log("Moving to patrol point: " + point.name);

            bool result = Host.ComeTo(point.x, point.y, point.z);
            isMoving = false;

            return result;
        }

        private void TryDashing()
        {
            bool isDashActive = IsAnyBuffExists(Buffs.Dash);

            int mpp = Host.me.mpp;
            int minMpp = (settings.FightAggroMobs) 
                ? 50 : 20;


            if (!InCombat() && !Host.me.isGlobalCooldown && mpp > minMpp && !isDashActive && !IsAnyBuffExists(Buffs.Freerunner))
            {
                Host.UseSkill(Skills.Dash);
            }

            else if (isDashActive && mpp <= (minMpp - 10))
            {
                CancelAnyBuffs(Buffs.Dash);
            }
        }

        private void TryWarping(double destX, double destY, double destZ)
        {
            var point = new Point3D(GetRayCast(18));
            var landingZone = new RoundZone(destX, destY, 8);

            double x = point.X,
                   y = point.Y,
                   z = point.Z, polyZ = 0;


            var poly = gps.GetAllGpsPolygons().Find(p => p.PointInZone(x, y));

            if (poly != null)
            {
                polyZ = poly.points.OrderBy(p => Host.dist(p.x, p.y, p.z)).First().z;
            }


            bool isCastReady = ((InNavMesh(x, y, z) && (Host.me.Z + 3.5) > Host.getZFromHeightMap(x, y)) 
                            || (IsPointInMesh(x, y) 
                            && polyZ != 0 && (Host.me.Z + 3.5) > polyZ))

                            && Host.me.dist(x, y) < Host.me.dist(destX, destY)
                            && GetNavDist(x, y, z, destX, destY, destZ) < GetNavDist(destX, destY, destZ)
                            && landingZone.PointInZone(point.X, point.Y);


            if (isCastReady && Host.UseSkill(Skills.Teleportation))
            {
                // Add to cooldown
            }
        }

        private void TryQuickstep()
        {
            int minMpp = (settings.FightAggroMobs) 
                ? 20 : 10;

            bool isQuickstepReady = !IsAnyBuffExists(Buffs.QuickstepBuffs) 
                
                && (Host.me.mp > minMpp)
                && !Host.me.isGlobalCooldown;


            if (isQuickstepReady)
            {
                Host.UseSkill(Skills.Quickstep);
            }
        }

        private void TryFreerun()
        {
            bool isFreerunReady = !IsAnyBuffExists(Buffs.Freerunner) 
                
                && (Host.me.mpp > 10) 
                && (Host.skillCooldown(Skills.Freerunner) == 0)
                && !Host.me.isGlobalCooldown;
                

            if (isFreerunReady)
            {
                Host.UseSkill(Skills.Freerunner);
            }
        }

        private void TryCometsBoon()
        {
            int minMpp = (settings.FightAggroMobs)
                ? 40 : 20;

            var isCometReady = (Host.skillCooldown(Skills.CometsBoon) == 0)

                && (Host.me.mpp > minMpp)
                && !Host.me.isGlobalCooldown;


            if (isCometReady)
            {
                Host.UseSkill(Skills.CometsBoon);
            }
        }

        private void TakeMiningQuest()
        {
            if (!IsQuestActive(6779) && Host.StartQuest(6779))
            {
                Utils.Delay(850, token);
            }
        }


        private void OnLaborAmountChanged(int count)
        {
            stats.LaborBurned += Math.Abs(count);

            Utils.InvokeOn(UI, () => UI.lbl_LaborRemaining.Text = Host.me.laborPoints.ToString());
        }

        private void OnNewInvItem(Item item, int count)
        {
            if (!MiningNodes.IsProduct(item.id))
                return;

            // Add item to box
            UI.AddToMined(item, count);
        }

        private void OnSkillCasting(Creature obj, SpawnObject obj2, Skill skill, double x, double y, double z)
        {
            if (obj.type == BotTypes.Player && obj2 == Host.me && skill.id == 20256)
            {
                stats.SuspectReports += 1;
                UI.GameLog($"You have been reported by: {obj.name}");
            }
        }

        private void OnChatMessage(ChatType chatType, string text, string sender)
        {
            if (chatType == ChatType.Whisper)
            {
                stats.WhispersReceived += 1;
                UI.GameLog($"From {sender}: {text}");
            }
        }

        private void OnTargetChanged(Creature obj1, Creature obj2)
        {
            if (obj1.type == BotTypes.Player && obj2 == Host.me)
            {
                UI.GameLog($"Targetted by {obj1.name}");
            }
        }

        #region Helpers

        private void MovingWatch(SpawnObject obj) => MovingWatch(obj.X, obj.Y, obj.Z);

        private bool IsWarpingEnabled()
        {
            return settings.UseTeleportation && Host.isSkillLearned(Skills.Teleportation);
        }

        private bool IsQuickstepEnabled()
        {
            return settings.UseQuickstep && Host.isSkillLearned(Skills.Quickstep);
        }

        private bool IsFreerunEnabled()
        {
            return settings.UseFreerunner && Host.isSkillLearned(Skills.Freerunner);
        }

        private bool IsCometBoonEnabled()
        {
            return settings.UseCometsBoon && Host.isSkillLearned(Skills.CometsBoon);
        }

        private bool IsQuestTakeLocked()
        {
            return (!settings.BeginDailyQuest || IsQuestActive(6779) || GetProfLevel(13) < 50000);
        }

        private bool IsPointInMesh(double x, double y)
        {
            return gps.GetAllGpsPolygons().Any(p => p.PointInZone(x, y));
        }

        private List<GpsPoint> GetPatrolPoints()
        {
            return gps.GetAllGpsPoints().Where(p => p.name.ToLower().Contains("patrol")).ToList();
        }

        private bool AnyPatrolPoints()
        {
            return GetPatrolPoints().Count() > 0;
        }


        private void CancelBoosts()
        {
            CancelAnyBuffs(Buffs.Dash);
        }

        private void HookEvents()
        {
            Host.onLaborAmountChanged += OnLaborAmountChanged;
            Host.onNewInvItem += OnNewInvItem;
            Host.onSkillCasting += OnSkillCasting;
            Host.onChatMessage += OnChatMessage;
            Host.onTargetChanged += OnTargetChanged;
        }
        
        private void UnhookEvents()
        {
            Host.onLaborAmountChanged -= OnLaborAmountChanged;
            Host.onNewInvItem -= OnNewInvItem;
            Host.onSkillCasting -= OnSkillCasting;
            Host.onChatMessage -= OnChatMessage;
            Host.onTargetChanged -= OnTargetChanged;
        }

        private void SetState(State state)
        {
            this.state = state;
        }

        #endregion
    }
}
