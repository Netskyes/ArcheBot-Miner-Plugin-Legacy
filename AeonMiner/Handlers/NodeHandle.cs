using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Handlers
{
    using Data;

    public class NodeHandle : Helpers
    {
        private Host Host { get; set; }
        private DoodadObject node;

        public NodeHandle(DoodadObject node, Host host) : base(host)
        {
            Host = host;
            this.node = node;

            BeginPhase = node.phaseId;
        }
        

        public DoodadObject Get()
        {
            return node;
        }

        public uint BeginPhase { get; set; }

        public double X
        {
            get { return (Exists()) ? node.X : 0; }
        }

        public double Y
        {
            get { return (Exists()) ? node.Y : 0; }
        }

        public double Z
        {
            get { return (Exists()) ? node.Z : 0; }
        }

        public double Dist()
        {
            return Exists() ? Host.dist(node) : 0;
        }

        public double NavDist()
        {
            return Exists() ? GetNavDist(node) : 0;
        }

        public bool Mine()
        {
            if (!Exists())
                return false;

            var uses = node.getUseSkills();

            return uses.Count > 0 && Host.UseDoodadSkill(uses.First().id, node);
        }

        public bool CanMine()
        {
            return Exists() && node.getUseSkills().Count > 0;
        }

        public bool Exists()
        {
            return node != null && Host.isExists(node);
        }

        public bool IsBusy()
        {
            return Exists() && Host.getCreatures().Any
                (c => (c.type == BotTypes.Player) && (c != Host.me) && (c.castObject == node));
        }

        public bool IsFocus()
        {
            return Exists() && Host.me.castObject == node;
        }

        public bool IsFortunaVein(bool beginPhase = false)
        {
            return Exists() 
                && MiningNodes.Veins.Fortuna.Contains((!beginPhase) ? node.phaseId : BeginPhase);
        }

        public bool IsUnidentifiedVein(bool beginPhase = false)
        {
            return Exists() 
                && MiningNodes.Veins.Unidentified.Contains((!beginPhase) ? node.phaseId : BeginPhase);
        }

        public override int GetHashCode()
        {
            return node.GetHashCode();
        }
    }
}
