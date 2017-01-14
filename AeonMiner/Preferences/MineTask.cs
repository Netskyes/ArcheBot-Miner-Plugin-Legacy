using System.Xml.Serialization;

namespace AeonMiner
{
    public class MineTask
    {
        [XmlAttribute("Name")]
        public string Name = string.Empty;
        public string MiningZone = string.Empty;
        public string NearbyDistrict = string.Empty;
    }
}
