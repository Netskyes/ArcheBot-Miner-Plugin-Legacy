using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AeonMiner.Navigation
{
    using Properties;

    public static class Maps
    {
        private static readonly IEnumerable<ZoneMap> maps = new List<ZoneMap>() 
        {
            new ZoneMap("Excavation Site", Resources.Excavation_Site, Resources.Excavation_Site_M ),
            new ZoneMap("Granite Quarry", Resources.Granite_Quarry, Resources.Granite_Quarry_M),
            new ZoneMap("Gnawbones Cave", Resources.Gnawbones_Cave, Resources.Gnawbones_Cave_M),
            new ZoneMap("Halo Hollow", Resources.Halo_Hollow, Resources.Halo_Hollow_M),
            new ZoneMap("Red Moss Cave", Resources.Red_Moss_Cave, Resources.Red_Moss_Cave_M)
        };

        public static IEnumerable<ZoneMap> GetAll()
        {
            foreach (var map in maps.Concat(GetLocal())) 
            {
                yield return map;
            }
        }

        public static IEnumerable<ZoneMap> GetLocal()
        {
            string[] maps = { };

            try
            {
                maps = Directory.GetFiles($"{Paths.Zones}", "*.db3");
            }
            catch
            {
            }

            foreach (var m in maps)
            {
                var name = Path.GetFileNameWithoutExtension(m);
                var temp = new ZoneMap(name, $"{name}.db3", $"{name}.ABMesh");

                yield return temp;
            }
        }

        public static ZoneMap GetMap(string name)
        {
            return GetAll().FirstOrDefault(m => m.Name == name) as ZoneMap;
        }
    }
}
