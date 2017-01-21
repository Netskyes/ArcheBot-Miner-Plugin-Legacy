using System;
using System.IO;

namespace AeonMiner
{
    public static class Paths
    {
        private static string[] structure = { "Zones", @"Zones\Meshes", "Logs", "Settings" };

        public static string[] GetStructure
        {
            get { return structure; }
        }

        public static string Folder
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\AeonMiner"); }
        }

        public static string PluginPath
        {
            get { return Folder + @"\"; }
        }

        public static string Zones
        {
            get { return Path.Combine(PluginPath, @"Zones\"); }
        }

        public static string Meshes
        {
            get { return Path.Combine(PluginPath, @"Zones\Meshes\"); }
        }

        public static string Logs
        {
            get { return Path.Combine(PluginPath, @"Logs\"); }
        }

        public static string Settings
        {
            get { return Path.Combine(PluginPath, @"Settings\"); }
        }

        public static string PackageFile
        {
            get { return PluginPath + "package.json";  }
        }
    }
}
