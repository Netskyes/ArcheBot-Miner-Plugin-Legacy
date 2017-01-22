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
        private Host Host { get; set; }

        public BaseModule(Host host) : base(host)
        {
            Host = host;
        }

        private Window UI
        {
            get { return (Window)Host.UIContext.Instance; }
        }

        // Tokens
        private Task loopTask;
        private CancellationTokenSource ts;
        private CancellationToken token;

        // Preferences
        private MineTask mineTask { get; set; }
        private Settings settings { get; set; }

        // Modules
        private GpsModule gps { get; set; }
        private CombatModule combat { get; set; }
        private MiningModule mining { get; set; }

        
        private bool Setup()
        {
            // Fetch preferences
            mineTask = UI.GetTask();
            settings = UI.SaveSettings() ?? UI.GetSettings();

            // Initialize modules
            gps = new GpsModule(Host);
            mining = new MiningModule(settings, Host, gps, token);
            combat = new CombatModule(settings, Host, gps, token);


            return Initialize();
        }

        private void BeginLoop() => loopTask = Task.Run(() => Loop(), token);


        public async void Start()
        {
            // Generate token
            ts = new CancellationTokenSource();
            token = ts.Token;

            // Lock button
            UI.UpdateButtonState("Loading...", false);


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
            UI.UpdateButtonState("Stopping...", false);

            CancelActions();
            

            UI.ButtonSwitch = false;
            UI.UpdateButtonState("Start Mining");
        }


        public void CancelActions()
        {
            ts.Cancel();

            // Primitives
            Host.CancelMoveTo();
            Host.CancelSkill();
            Host.RotateLeft(false);
            Host.RotateRight(false);
            Host.MoveBackward(false);
            Host.MoveForward(false);

            CancelBoosts();
            UnhookEvents();

            // Wait for task to terminate
            while (loopTask.Status == TaskStatus.Running)
            {
                Utils.Sleep(10);
            }
        }
    }
}
