using System;
using System.IO;
using System.Linq;
using System.Threading;
using ArcheBot.Bot.Classes;

namespace AeonMiner
{
    using UI;
    using Modules;

    public sealed class Host : Core
    {
        private bool initStop = false;

        public static string PluginVersion()
        {
            return "3.0.0";
        }

#if !DEBUG
        /// <summary>
        /// Loads plugin into main app domain.
        /// </summary>
        public static bool isReleaseVersion = true;
        public static int storePluginId = 20;
#endif
        public UIContext UIContext { get; private set; }
        public BaseModule BaseModule { get; private set; }

        
        public bool IsGameReady()
        {
            return me != null && gameState == GameState.Ingame;
        }

        private void Initialize()
        {
            Paths.Validate();

            UIContext = new UIContext(new Window(this));
            BaseModule = new BaseModule(this);

            // Console Text Color
            LogSetColor(System.Drawing.Color.White);
        }

        public void PluginRun()
        {
            if (!IsGameReady())
            {
                Log("Loading or not in game...");

                while (!IsGameReady()) Utils.Sleep(50);
            }

            ClearLogs();
            Log("AeonMiner v." + PluginVersion());


            Debug();
            Initialize();


            UIContext.Load();

            try
            {
                while (!initStop) Utils.Sleep(50);
            }
            catch (ThreadAbortException)
            {
                // Skip
            }
            finally
            {
                UIContext.Unload();
            }
        }

        public void PluginStop()
        {
            initStop = true;

            BaseModule.CancelActions();
        }


        // DEBUG
        private void Debug()
        {
        }
    }
}
