using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeonMiner.Modules
{
    using Preferences;

    internal class CombatModule
    {
        private Host Host { get; set; }
        private Settings settings;
        private CancellationToken token;

        public CombatModule(Settings settings, Host host, GpsModule gps, CancellationToken token)
        {
            Host = host;
            this.token = token;
            this.settings = settings;
        }
    }
}
