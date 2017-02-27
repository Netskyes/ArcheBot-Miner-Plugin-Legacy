using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AeonMiner.Preferences
{
    [Serializable]
    public class Settings
    {
        public int MinLaborPoints = 25;

        public bool AutoStart;
        public bool RunPlugin;
        public bool SkipBusyNodes;
        public bool FightAggroMobs;
        public bool AutoLevelUp;
        public bool BeginDailyQuest;
        public bool FinishDailyQuest;
        public bool RemoveSuspect;
        public bool UseDash;
        public bool UseFreerunner;
        public bool UseTeleportation;
        public bool UseQuickstep;
        public bool UseCometsBoon;
        
        public string TaskName = string.Empty;
        public string TravelMount = string.Empty;
        public string FinalAction = string.Empty;
        public string PluginRunName = string.Empty;

        
        [XmlArrayItem("Name")]
        public List<string> CleanItems;

        public Settings()
        {
            CleanItems = new List<string>();
        }
    }
}
