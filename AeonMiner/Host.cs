using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner
{
    using UI;
    using Modules;

    public sealed class Host : Core
    {
        #region Props & Fields

        /// <summary>
        /// Singleton instance access of host.
        /// </summary>
        public static Host Instance { get; private set; }
        
        /// <summary>
        /// Main UI context cotroller.
        /// </summary>
        public UIContext UIContext { get; private set; }

        /// <summary>
        /// Module base manager.
        /// </summary>
        public BaseModule BaseModule { get; private set; }


        private bool initStop = false;

        public static string PluginVersion
        {
            get { return "1.0.0.2"; }
        }

        #endregion

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

        private void Initialize()
        {
            CheckResources();

            Instance = this;
            UIContext = new UIContext(new Window());
            BaseModule = new BaseModule();

            // Console Text Color
            LogSetColor(System.Drawing.Color.White);
        }

        public void PluginRun()
        {
            Log("Loading or not in game...");

            while (gameState != GameState.Ingame || me == null)
                Utils.Sleep(50);

            ClearLogs();
            Log("AeonMiner v." + PluginVersion);


            Initialize();
            Debug();


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
            string file = Paths.PluginPath + "dump.txt";
            var veins = sqlCore.sqlDoodadAlmighties.Where(d => d.Value.name.Contains("Vein") && d.Value.groupId == 4);

            foreach (var vein in veins)
            {
                File.AppendAllText(file, "[" + vein.Value.name + " " + vein.Value.id + "]: " + vein.Value.groupId + "\n");

                foreach (var u in vein.Value.phases)
                {
                    File.AppendAllText(file, "\tPhase: " + u.name + "\n");

                    foreach (var s in u.useSkills)
                    {
                        File.AppendAllText(file, "\t\tSkill -> " + s.name + " " + s.id + "\n");
                    }
                }
                File.AppendAllText(file, "---\n");

                Utils.Sleep(10);
            }
        }
    }
}
