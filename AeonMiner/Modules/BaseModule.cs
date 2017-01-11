using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeonMiner.Modules
{
    using UI;
    using Preferences;

    public sealed partial class BaseModule
    {
        private Host Host
        {
            get { return Host.Instance; }
        }

        private Window UI
        {
            get { return (Window)Host.UIContext.Instance; }
        }


        // Modules
        private GpsModule gps;
        private CombatModule combat;
        private MiningModule mining;

        // Preferences
        private MineTask mineTask;
        private Settings settings;

        private CancellationTokenSource ts;
        private CancellationToken token;


        private bool Setup()
        {
            // Init modules
            gps = new GpsModule();
            mining = new MiningModule(token);
            combat = new CombatModule(token);

            // Prefs
            mineTask = UI.GetTask();
            settings = UI.SaveSettings() ?? UI.GetSettings();
            

            return Initialize();
        }

        private void BeginLoop() => Task.Run(() => Loop(), token);


        public async void Start()
        {
            // Generate token
            ts = new CancellationTokenSource();
            token = ts.Token;

            // Lock button
            UI.UpdateButtonState("...", false);


            bool result = await Task.Run(() => Setup(), token);

            if (result)
            {
                BeginLoop();

                UI.ButtonSwitch = true;
                UI.UpdateButtonState("Stop");
            }
            else
            {
                UI.UpdateButtonState("Start Mining", true);
            }
        }

        public void Stop()
        {
            // Lock button
            UI.UpdateButtonState("...", false);

            CancelActions();
            

            UI.ButtonSwitch = false;
            UI.UpdateButtonState("Start Mining");
        }


        public void CancelActions()
        {
            ts.Cancel();

            Host.CancelMoveTo();
            Host.CancelSkill();
            Host.RotateLeft(false);
            Host.RotateRight(false);
            Host.MoveBackward(false);
            Host.MoveForward(false);
        }
    }
}
