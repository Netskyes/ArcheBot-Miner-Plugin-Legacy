using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Modules
{
    using Enums;
    using Helpers;

    internal class GpsModule : Gps
    {
        private Host Host { get; set; }

        public GpsModule(Host host) : base(host)
        {
            Host = host;
        }


        public bool Load(string mapName)
        {
            var zoneMap = MapsHelper.GetMap(mapName);

            if (zoneMap == null)
            {
                Host.Log("Failed to acquire zone map!");
                return false;
            }

            if (!zoneMap.MeshExists())
            {
                Host.Log("Failed to load or missing mesh map!");
                return false;
            }


            bool success = false;

            switch (zoneMap.MapUseType)
            {
                case MapUseType.Local:
                    success = LoadDataBase(zoneMap.GetMapPath());
                    Host.LoadNavMesh(zoneMap.GetMeshPath(), false);
                    break;

                case MapUseType.Internal:
                    success = LoadDataBase(zoneMap.GetByteMap());
                    Host.LoadNavMesh(zoneMap.GetByteMesh(), false);
                    break;
            }

            if (success)
            {
                MeshEnabled = true;
                return true;
            }

            return false;
        }

        public bool MeshEnabled
        {
            get { return Host.forceNavMeshMovements; }
            set
            {
                Host.forceNavMeshMovements = value;
            }
        }


        public List<GpsPoint> GetPointsByName(string match)
        {
            return GetAllGpsPoints().Where(p => p.name.ToLower().Contains(match.ToLower())).ToList();
        }
    }
}
