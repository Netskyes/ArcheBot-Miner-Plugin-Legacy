using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArcheBot.Bot.Classes;
using System.Diagnostics;

namespace AeonMiner.Modules
{
    using Data;
    using Handlers;
    using Preferences;

    internal class MiningModule : Helpers
    {
        private Settings settings;
        private CancellationToken token;

        private Host Host
        {
            get { return Host.Instance; }
        }

        public MiningModule(Settings settings, CancellationToken token)
        {
            this.token = token;
            this.settings = settings;
        }

        private NodeHandle node;
        private int prevHash { get; set; }
        private int skipNode { get; set; }
        private List<int> ignoreNodes = new List<int>();
        private List<uint> ignorePhases = new List<uint>();

        private bool isMiningNode;
        private bool isMovingToNode;


        public bool GetTarget()
        {
            GetNode();

            return node != null && node.Exists();
        }

        public bool MineVein()
        {
            while (!node.CanMine())
            {
                if (!node.Exists() || !token.IsAlive())
                    return false;

                Utils.Delay(50, token);
            }


            isMiningNode = true;

            Task.Run(() => MiningWatch(), token);


            uint phaseId = node.Get().phaseId;
            bool isMined = node.Mine();

            isMiningNode = false;


            if (!token.IsAlive())
                return false;

            if (isMined)
            {
                while (node.Exists() && node.Get().phaseId == phaseId)
                {
                    Utils.Delay(50, token);
                }
            }

            return isMined;
        }

        private void MiningWatch()
        {
            while (isMiningNode && token.IsAlive())
            {
                try
                {
                    if ((settings.FightAggroMobs && InCombat()))
                    {
                        Host.CancelSkill();
                        break;
                    }
                }
                catch
                {
                }


                Utils.Delay(50, token);
            }
        }


        public bool MoveToTarget()
        {
            if (Host.dist(node.Get()) < 2)
                return true;


            isMovingToNode = true;

            Task.Run(() => MovingWatch(), token);
            

            bool result = Host.ComeTo(node.Get(), Utils.RandomDouble(0.8, 1.2));
            isMovingToNode = false;

            return result;
        }

        private void MovingWatch()
        {
            while (isMovingToNode && token.IsAlive())
            {
                try
                {
                    if (!node.Exists() || (settings.FightAggroMobs && InCombat()))
                    {
                        Host.CancelMoveTo();
                        break;
                    }

                    if (settings.SkipBusyNodes && node.IsBusy())
                    {
                        skipNode = node.GetHashCode();

                        Host.CancelMoveTo();
                        break;
                    }
                }
                catch
                {
                }


                Utils.Delay(50, token);
            }
        }


        private void GetNode()
        {
            if (node != null)
            {
                prevHash = node.GetHashCode();
            }


            node = null;

            var nodes = Host.getDoodads().Where
                (d => MiningNodes.Exists(d.phaseId) 
                && skipNode != d.GetHashCode() 
                && !ignorePhases.Contains(d.phaseId) && !ignoreNodes.Contains(d.GetHashCode()));


            int count = nodes.Count();

            if (count < 1)
                return;


            if (count == 1)
            {
                node = new NodeHandle(nodes.First());
                return;
            }


            // Consider Z later
            var bestNode = nodes.OrderBy(d => Host.dist(d)).First();

            node = new NodeHandle(bestNode);
        }

        public double DistToTarget()
        {
            return (node != null && node.Exists()) ? Host.dist(node.Get()) : 0;
        }
    }
}
