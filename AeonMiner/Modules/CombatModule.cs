using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeonMiner.Modules
{
    internal class CombatModule
    {
        private CancellationToken token;

        private Host Host
        {
            get { return Host.Instance; }
        }

        public CombatModule(CancellationToken token)
        {
            this.token = token;
        }


    }
}
