using System;

namespace AeonMiner
{
    [Serializable]
    public class Telemetry
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}
