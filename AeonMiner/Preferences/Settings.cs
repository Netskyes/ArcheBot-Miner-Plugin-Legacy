using System;
using System.Collections.Generic;

namespace AeonMiner.Preferences
{
    [Serializable]
    public class Settings
    {
        public bool AutoStart;
        public bool RunPlugin;

        public string TaskName = string.Empty;
        public string TravelMount = string.Empty;
        public string FinalAction = string.Empty;
        public string PluginRunName = string.Empty;

        public Settings()
        {

        }
    }
}
