using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Modules
{
    using Enums;
    using Navigation;

    internal class GpsModule
    {
        private Host Host
        {
            get { return Host.Instance; }
        }

        public GpsModule()
        {
            gps = new Gps(Host);
        }

        private Gps gps;


        public bool Load(string zoneName)
        {
            var zoneMap = Maps.GetMap(zoneName);

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
                    success = gps.LoadDataBase(zoneMap.GetMapPath());
                    break;

                case MapUseType.Internal:
                    success = gps.LoadDataBase(zoneMap.GetByteMap());
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
    }
}
