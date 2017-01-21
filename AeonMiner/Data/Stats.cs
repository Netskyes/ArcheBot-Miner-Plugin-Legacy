using System;

namespace AeonMiner.Data
{
    [Serializable]
    public class Stats
    {
        public string ZoneName = string.Empty;

        public long RunTime;
        public int LaborStartedWith;
        public int LaborBurned;
        public int VeinsMined;
        public int FortunaVeins;
        public int UnidentifiedVeins;
        public double AvgMinedPerHour;

        public Stats()
        {

        }

        public void SaveToDatabase()
        {
            // SQLite
        }
    }
}
