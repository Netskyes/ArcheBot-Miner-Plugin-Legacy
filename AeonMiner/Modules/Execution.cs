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
        private MiningStats stats;
        private MineTask mineTask;
        
        private State state = State.Check;
        private NodeHandle node;

        private bool isMoving;
        private bool isMining;

        /// <summary>
        /// Initialize runtime.
        /// </summary>
        private bool Initialize()
        {
            if (settings.TaskName == string.Empty)
            {
                Host.Log("Please select a mining task.");
                return false;
            }

            if (!MakeTask(settings.TaskName))
            {
                Host.Log("Something went wrong creating current mining task.");
                return false;
            }

            foreach (var vein in mineTask.IgnoreVeins)
            {
                mining.AddIgnorePhases(MiningNodes.GetPhasesByName(vein));
            }


            // Reset!
            state = State.Check;

            // Events
            HookEvents();
            
            // Start tasks
            Task.Run(() => RunTimer(), token);

            
            return true;
        }

        private bool MakeTask(string name)
        {
            if (!UI.MiningTasks.ContainsKey(name))
                return false;


            mineTask = UI.MiningTasks[name];

            if (mineTask == null || !gps.Load(mineTask.MiningZone))
                return false;


            if ((stats == null) || (stats.ZoneName != mineTask.MiningZone) || UI.ResetStats())
            {
                stats = new MiningStats(UI);
                stats.ZoneName = mineTask.MiningZone;
                stats.LaborStartedWith = Host.me.laborPoints;
            }


            return mineTask != null && stats != null;
        }

        private void HandleStopRequest()
        {
            Stop(); 

            // Post actions
            if (settings.RunPlugin && settings.PluginRunName != string.Empty)
            {
                Host.RunPlugin(settings.PluginRunName);
            }
        }

        


        private void Execute()
        {
            /// ... Combat Check Here!
            /// ... Random sleep here as well, except when in combat.

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
            if (Host.me.laborPoints < settings.MinLaborPoints)
            {
                throw new StopException("Out of labor points, stopping.");
            }


            SetState(State.Search);
        }

        private void Search()
        {
            if ((node = mining.GetNode()) == null)
            {
                if (IsPatrolPointsExists())
                {
                    BeginPatrol();
                }
                
                return;
            }

            if (!mining.IsNodeCheck())
                return;


            SetState(State.Move);
        }

        private void Move()
        {
            if (node.Dist() < 1.5)
            {
                Utils.Delay(0, 250, token);
                SetState(State.Mine);

                return;
            }

            Host.SetLastError(LastError.Unknown);


            isMoving = true;

            Task.Run(() => MovingWatch(node.Get()), token);


            bool result = mining.MoveToNode();
            isMoving = false;

            if (!token.IsAlive())
                return;


            if (!result)
            {
                SetState(State.Check);
                
                if (Host.GetLastError() == LastError.MoveUnknownError)
                {
                    mining.SkipNode();
                }

                return;
            }


            SetState(State.Mine);
            Utils.Delay(0, 350, token);
        }

        private void Mine()
        {
            if (!IsQuestTakeLocked() && node.IsFortunaVein())
            {
                TakeMiningQuest();
            }


            isMining = true;

            Task.Run(() => MiningWatch(), token);


            bool result = mining.MineVein();
            isMining = false;

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

                Utils.Delay(250, 850, token);
            }
            else
            {
                Log(Host.GetLastError().ToString());
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

        private void MiningWatch()
        {
            while (isMining && token.IsAlive())
            {
                try
                {
                    
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


        // Add "Come Inside Mesh"

        private void BeginPatrol()
        {
            isMoving = true;
            MoveUnless(() => mining.GetNode() != null || (settings.FightAggroMobs && InCombat()));


            Log("Patrolling...");

            bool result = MoveToPatrolPoint();
            isMoving = false;


            // ... Do something!
        }

        private bool MoveToPatrolPoint()
        {
            if (!IsPatrolPointsExists())
                return false;

            var point = GetPatrolPoints().OrderByDescending(p => GetNavDist(p.x, p.y, p.z)).First();


            return Host.ComeTo(point.x, point.y, point.z);
        }

        private void RotateTo(double x, double y, Func<bool> eval = null)
        {
            Func<int> GetAngle = () =>
            {
                int angle = Host.angle(Host.me, x, y); return ((angle / 180) * 360) - angle;
            };


            if (GetAngle() < 0)
            {
                Host.RotateRight(true);

                while (token.IsAlive() && GetAngle() < -6 && Host.rotateRightState)
                {
                    if (eval != null && !eval.Invoke())
                        break;


                    Utils.Delay(50, token);
                }
            }
            else
            {
                Host.RotateLeft(true);

                while (token.IsAlive() && GetAngle() > 6 && Host.rotateLeftState)
                {
                    if (eval != null && !eval.Invoke())
                        break;


                    Utils.Delay(50, token);
                }
            }

            // Reset states
            Host.RotateLeft(false);
            Host.RotateRight(false);
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

        private bool MakeCampfire()
        {
            uint[] phases = { 17430, 17431, 17438, 17439, 17462 };

            Func<DoodadObject> dood = () => Host.getDoodads().Where
                (d => (d.id == 6591) && phases.Contains(d.phaseId) && InNavMesh(d) && (Host.dist(d) < 14)).OrderBy(d => Host.dist(d)).FirstOrDefault();


            if (dood() == null)
            {
                Host.SetLastError(LastError.Unknown);

                if (!IsItemExists(new uint[] { 27735, 30905, 8017 }))
                    return false;


                var point = GetRayCast(1.2);

                if (!Host.UseItem(27735, point[0], point[1], Host.me.Z))
                    return false;
                

                Utils.Delay(1250, 1850, token);
            }


            var camp = dood();

            while (token.IsAlive() && camp != null)
            {
                switch (camp.phaseId)
                {
                    case 17430:
                        Host.UseDoodadSkill(21960, dood(), true);
                        break;
                    case 17431:
                        Host.UseDoodadSkill(21962, dood(), true);
                        break;
                    case 17438:
                        Host.UseDoodadSkill(21989, dood(), true);
                        break;

                    case 17439:
                    case 17462:
                        return true;
                }

                Utils.Delay(1250, 1850, token);
            }

            return false;
        }

        private DoodadObject FindCampfire()
        {
            return Host.getDoodads().Find(d => InNavMesh(d) && d.id == 6591 && (d.phaseId == 17462 || d.phaseId == 17439));
        }

        #region Events Handler

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

        #endregion

        #region Helpers

        private void SetState(State state) => this.state = state;

        private bool IsWarpingEnabled() => settings.UseTeleportation && Host.isSkillLearned(Skills.Teleportation);
        private bool IsQuickstepEnabled() => settings.UseQuickstep && Host.isSkillLearned(Skills.Quickstep);
        private bool IsFreerunEnabled() => settings.UseFreerunner && Host.isSkillLearned(Skills.Freerunner);
        private bool IsCometBoonEnabled() => settings.UseCometsBoon && Host.isSkillLearned(Skills.CometsBoon);
        private bool IsQuestTakeLocked() => (!settings.BeginDailyQuest || IsQuestActive(6779) || GetProfLevel(13) < 50000);

        private bool IsPointInMesh(double x, double y) => gps.GetAllGpsPolygons().Any(p => p.PointInZone(x, y));
        private List<GpsPoint> GetPatrolPoints() => gps.GetPointsByName("patrol");
        private bool IsPatrolPointsExists() => (GetPatrolPoints().Count() > 0);

        private void MovingWatch(SpawnObject obj) => MovingWatch(obj.X, obj.Y, obj.Z);


        private void MoveUnless(Func<bool> eval)
        {
            Task.Run(() =>
            {
                while (isMoving && token.IsAlive())
                {
                    try
                    {
                        if (eval.Invoke())
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

            }, token);
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

        #endregion
    }
}
