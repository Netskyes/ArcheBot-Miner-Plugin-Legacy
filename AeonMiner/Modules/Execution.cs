using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Modules
{
    using Data;
    using Enums;
    using Handlers;

    public sealed partial class BaseModule : Helpers
    {
        private State state = State.Check;
        private bool isMoving;


        /// <summary>
        /// Initialize assets.
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


            // Default vars
            state = State.Check;

            return true;
        }

        /// <summary>
        /// Execution loop task.
        /// </summary>
        private void Loop()
        {
            while (!token.IsCancellationRequested)
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
            SetState(State.Search);
        }

        private void Search()
        {
            if (mining.GetTarget())
            {
                SetState(State.Move);
            }
        }

        private void Move()
        {
            if (mining.DistToTarget() < 2)
            {
                SetState(State.Mine);
                return;
            }


            isMoving = true;

            Task.Run(() => MovingWatch(), token);


            bool result = mining.MoveToTarget();
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
                SetState(State.Check);
            }
        }

        private void Mine()
        {
            if (mining.MineVein())
            {
                if (settings.AutoLevelUp && CanUpgradeLevel(13))
                {
                    UpgradeLevel(13);
                }

                Utils.Delay(850, 1450, token);


                SetState(State.Check);
                return;
            }


            SetState(State.Check);
        }



        private void MovingWatch()
        {
            while (isMoving && token.IsAlive())
            {
                try
                {
                    if (mining.DistToTarget() > 5.5)
                    {
                        TryDashing();
                    }
                    else
                    {
                        CancelAnyBuffs(Buffs.Dash);
                    }
                }
                catch
                {
                }


                Utils.Delay(50, token);
            }

            CancelBoosts();
        }

        private void TryDashing()
        {
            bool isDashActive = IsAnyBuffExists(Buffs.Dash);

            int mpp = Host.me.mpp;
            int minMpp = (settings.FightAggroMobs) 
                ? 50 : 20;


            if (mpp > minMpp && !isDashActive && !IsAnyBuffExists(Buffs.Freerunner))
            {
                Host.UseSkill(16287);
            }

            else if (isDashActive && mpp <= (minMpp - 10))
            {
                CancelAnyBuffs(Buffs.Dash);
            }
        }

        private void CancelBoosts()
        {
            CancelAnyBuffs(Buffs.Dash);
        }

        #region Helpers

        private void SetState(State state)
        {
            this.state = state;
        }

        #endregion
    }
}
