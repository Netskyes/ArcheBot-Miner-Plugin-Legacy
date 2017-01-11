using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;
using System.Diagnostics;

namespace AeonMiner.Modules
{
    internal class MiningModule
    {
        private CancellationToken token;

        private Host Host
        {
            get { return Host.Instance; }
        }

        public MiningModule(CancellationToken token)
        {
            this.token = token;
        }

        public int skipNode { get; set; }
        public List<int> ignoreNodes = new List<int>();


        public DoodadObject GetNode()
        {
            var nodes = Host.getDoodads().Where
                (d => 
                d.dbAlmighty.groupId == 4
                && d.getUseSkills().Count > 0
                && skipNode != d.GetHashCode()
                && !ignoreNodes.Contains(d.GetHashCode()));


            int count = nodes.Count();

            if (count < 1)
            {
                return null;
            }

            if (count == 1)
            {
                return nodes.First();
            }


            return nodes.OrderBy(d => Host.dist(d)).First();
        }
    }
}
