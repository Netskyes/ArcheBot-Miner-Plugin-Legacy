using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeonMiner.Modules
{
    using UI;
    using Enums;
    using Configs;

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
        private Settings settings;

        // Modules
        private GpsModule gps;
        private CombatModule combat;
        private MiningModule mining;
        private BehaviorModule behavior;


        private bool StartUp()
        {
            // Fetch and save config
            settings = UI.SaveSettings();

            // Define modules
            gps = new GpsModule(Host);
            mining = new MiningModule(settings, Host, gps, token);
            combat = new CombatModule(settings, Host, gps, token);
            behavior = new BehaviorModule(settings, Host, gps, token);


            return Initialize();
        }

        private void BeginLoop() => (loopTask) = Task.Run(() => Loop(), token);


        public async void Start()
        {
            // Generate token
            ts = new CancellationTokenSource();
            token = ts.Token;

            // Lock button
            UI.UpdateButtonState("Loading...", false);


            bool result = await Task.Run(() => StartUp(), token);

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
                    Log(e.Message);
                    Task.Run(() => HandleStopRequest());

                    return;
                }
                catch
                // Error exception
                (Exception e) { LogException(e); }


                Utils.Delay(50, token);
            }
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

        private void LogException(Exception e)
        {
            // Skip logging common exceptions
            if (e.GetType() == typeof(AggregateException))
                return;


            string exLog = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]: ";
            exLog += $"Message: {e.Message}{Environment.NewLine}";
            exLog += $"StackTrace: {e.StackTrace}{Environment.NewLine}";
            exLog += $"----{Environment.NewLine}";

            try
            {
                File.AppendAllText(Paths.Logs + $"{Host.me.name}@{Host.serverName()}.log", exLog);
            }
            catch
            {
            }
        }
    }
}
