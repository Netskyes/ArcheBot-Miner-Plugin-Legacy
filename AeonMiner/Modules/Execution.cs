using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Modules
{
    using Enums;

    public sealed partial class BaseModule
    {
        #region Fields

        private State state = State.Check;
        
        #endregion

        private bool Initialize()
        {
            if (!gps.Load(mineTask.MiningZone))
                return false;
            


            return true;
        }


        private void Loop()
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Execute();
                }
                catch (Exception e)
                {
                    Host.Log(e.Message + " " + e.StackTrace);
                }


                Utils.Delay(50, token);
            }
        }

        private void Execute()
        {
            switch (state)
            {
                case State.Check:
                    Check();
                    break;
            }
        }

        

        private void Check()
        {
        
        }
    }
}
