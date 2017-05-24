using System;
using System.IO;

namespace AeonMiner
{
    public static class Paths
    {
        /// <summary>
        /// Plugins/AeonMiner
        /// </summary>
        public static string Folder
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\AeonMiner"); }
        }

        /// <summary>
        /// Plugins/AeonMiner/
        /// </summary>
        public static string Plugin
        {
            get { return Folder + @"\"; }
        }

        public static string Zones
        {
            get { return Path.Combine(Plugin, @"Zones\"); }
        }

        public static string Meshes
        {
            get { return Path.Combine(Plugin, @"Zones\Meshes\"); }
        }

        public static string Logs
        {
            get { return Path.Combine(Plugin, @"Logs\"); }
        }

        public static string Settings
        {
            get { return Path.Combine(Plugin, @"Settings\"); }
        }

        public static string[] Structure = { "Zones", @"Zones\Meshes", "Logs", "Settings" };


        public static void Validate()
        {
            if (!Directory.Exists(Folder))
            {
                try
                {
                    Directory.CreateDirectory(Folder);
                }
                catch
                {
                }
            }

            foreach (string name in Structure)
            {
                string path = Path.Combine(Plugin, name);

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
    }
}
