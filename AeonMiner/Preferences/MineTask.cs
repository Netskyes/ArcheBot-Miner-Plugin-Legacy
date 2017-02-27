using System.Collections.Generic;
using System.Xml.Serialization;

namespace AeonMiner
{
    public class MineTask
    {
        [XmlAttribute("Name")]
        public string Name = string.Empty;
        public string MiningZone = string.Empty;

        [XmlArrayItem("Name")]
        public List<string> IgnoreVeins;

        public int TaskTime;

        public bool UseTaskTime;
       

        public MineTask()
        {
            IgnoreVeins = new List<string>();
        }
    }
}
