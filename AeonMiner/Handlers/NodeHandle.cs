using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Handlers
{
    using Data;

    public class NodeHandle
    {
        private DoodadObject node;

        private Host Host
        {
            get { return Host.Instance; }
        }

        public NodeHandle(DoodadObject node)
        {
            this.node = node;
        }
        

        public DoodadObject Get()
        {
            return node;
        }

        public bool Mine()
        {
            var uses = node.getUseSkills();

            return node != null && uses.Count > 0 && Host.UseDoodadSkill(uses.First().id, node);
        }

        public bool CanMine()
        {
            return node != null && node.getUseSkills().Count > 0;
        }

        public bool Exists()
        {
            return node != null && Host.isExists(node);
        }

        public bool IsBusy()
        {
            return node != null && Host.getCreatures().Any(c => c.castObject == node);
        }

        public bool IsFortunaVein()
        {
            return MiningNodes.Veins.Fortuna.Contains(node.phaseId);
        }

        public override int GetHashCode()
        {
            return node.GetHashCode();
        }
    }
}
