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
        private Host Host { get; set; }
        private Settings settings;
        private CancellationToken token;
        private Gps gps;

        public MiningModule(Settings settings, Host host, GpsModule gps, CancellationToken token) : base(host)
        {
            Host = host;
            this.token = token;
            this.settings = settings;
            this.gps = gps;

            Initialize();
        }

        private NodeHandle node;
        private int prevHash { get; set; }

        private bool isMiningNode;
        private bool isMovingToNode;

        private List<int> skipNodes = new List<int>();
        private List<int> ignoreNodes = new List<int>();
        private List<uint> ignorePhases = new List<uint>();

        
        private void Initialize()
        {
            if (GetProfLevel(13) < 230000)
            {
                foreach (var phase in MiningNodes.Veins.Unidentified)
                {
                    ignorePhases.Add(phase);
                }
            }
        }

        public NodeHandle GetNode()
        {
            node = FindNode();

            return (node != null && node.Exists()) ? node : null;
        }

        public bool IsNodeCheck()
        {
            if (node == null || !node.Exists())
                return false;


            if (settings.SkipBusyNodes)
            {
                if (node.IsBusy())
                {
                    Log("Node busy, skipping...");
                    skipNodes.Add(node.GetHashCode());

                    return false;
                }
                else
                {
                    skipNodes.Clear();
                }
            }
            

            var edgePoint = gps.GetNearestPoint(node.X, node.Y, node.Z);
            var distPoint = (edgePoint != null) ? node.Get().dist(edgePoint.x, edgePoint.y, edgePoint.z) : 0;

            if (!InNavMesh(node.Get()) && distPoint > 8)
            {
                ignoreNodes.Add(node.GetHashCode());

                return false;
            }

            return true;
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


            bool isMined = node.Mine();
            isMiningNode = false;


            if (!token.IsAlive())
                return false;

            if (isMined)
            {
                while (node.Exists() && node.BeginPhase == node.Get().phaseId)
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
                    bool isCancel = (Host.me.isCasting && !node.CanMine()) 
                        || (settings.FightAggroMobs && InCombat());


                    if (isCancel)
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


        public bool MoveToNode()
        {
            if (Host.dist(node.Get()) < 2)
                return true;


            isMovingToNode = true;

            Task.Run(() => MovingWatch(), token);
            

            bool isComeTo = Host.ComeTo(node.X, node.Y, node.Z, Utils.RandomDouble(0.8, 1.2));
            isMovingToNode = false;

            return isComeTo;
        }

        private void MovingWatch()
        {
            while (isMovingToNode && token.IsAlive())
            {
                try
                {
                    bool isCancel = !node.Exists() 
                        || !node.CanMine() || (settings.FightAggroMobs && InCombat());


                    if (isCancel)
                    {
                        Log("Cancelling move: 1");
                        Host.CancelMoveTo();
                        break;
                    }

                    if (settings.SkipBusyNodes && node.IsBusy())
                    {
                        Log("Cancelling move: 2");
                        skipNodes.Add(node.GetHashCode());

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


        private NodeHandle FindNode()
        {
            if (node != null)
            {
                prevHash = node.GetHashCode();
            }


            node = null;

            var nodes = Host.getDoodads().Where(d => MiningNodes.Exists(d.phaseId)

                && !ignorePhases.Contains(d.phaseId)
                && !skipNodes.Contains(d.GetHashCode()) 
                && !ignoreNodes.Contains(d.GetHashCode())
                && GetNavDist(d) <= 200);


            int count = nodes.Count();

            if (count < 1)
                return null;


            DoodadObject temp = null;

            if (count == 1)
            {
                temp = nodes.First();
            }
            else
            {
                temp = nodes.OrderBy(d => GetNavDist(d)).ThenBy(d => Host.dist(d)).First();
            }


            return (new NodeHandle(temp, Host));
        }
    }
}
