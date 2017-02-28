using System;
using System.IO;
using System.Linq;
using System.Threading;
using ArcheBot.Bot.Classes;

namespace AeonMiner
{
    using UI;
    using Modules;
    using Utility;

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
        public static int storePluginId = 1;
#endif
        public UIContext UIContext { get; private set; }
        public BaseModule BaseModule { get; private set; }

        
        private void CheckResources()
        {
            if (!Directory.Exists(Paths.Folder))
            {
                try
                {
                    Directory.CreateDirectory(Paths.Folder);
                }
                catch
                {
                }
            }

            foreach (string name in Paths.GetStructure)
            {
                string path = Path.Combine(Paths.PluginPath, name);

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public bool IsGameReady()
        {
            return me != null && gameState == GameState.Ingame;
        }


        private void Initialize()
        {
            CheckResources();

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
