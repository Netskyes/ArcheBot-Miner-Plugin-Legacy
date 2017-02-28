using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ArcheBot.Bot.Classes;

namespace AeonMiner.Modules
{
    using Configs;

    internal class BehaviorModule : CoreHelper
    {
        private Host Host { get; set; }
        private Settings settings;
        private CancellationToken token;
        private Gps gps;

        public BehaviorModule(Settings settings, Host host, GpsModule gps, CancellationToken token) : base(host)
        {
            Host = host;
            this.token = token;
            this.settings = settings;
            this.gps = gps;
        }


        public void RandomLooking(Func<bool> eval)
        {
            SpawnObject viewpoint = null;

            var players = Host.getCreatures().Where
                (c => Host.dist(c) < 40
                && (c.type == BotTypes.Player || c.type == BotTypes.Npc));
                

            if (players.Count() > 0)
            {
                if (Utils.RandomNum(0, 1) < 1)
                {
                    players = players.OrderBy(c => Host.dist(c));
                }
                else
                {
                    players = players.OrderByDescending(c => Host.dist(c));
                }


                viewpoint = players.FirstOrDefault();
            }


            if (viewpoint != null)
            {
                RotateTo(viewpoint.X, viewpoint.Y, 20, eval);
            }
        }



        public void RotateTo(double x, double y, int angle, Func<bool> eval = null)
        {
            Func<int> GetAngle = () =>
            {
                int myAngle = Host.angle(Host.me, x, y); return ((myAngle / 180) * 360) - myAngle;
            };


            if (GetAngle() < 0)
            {
                Host.RotateRight(true);

                while (token.IsAlive() && GetAngle() < -angle && Host.rotateRightState)
                {
                    if (eval != null && eval.Invoke())
                        break;


                    Utils.Delay(50, token);
                }
            }
            else
            {
                Host.RotateLeft(true);

                while (token.IsAlive() && GetAngle() > angle && Host.rotateLeftState)
                {
                    if (eval != null && eval.Invoke())
                        break;


                    Utils.Delay(50, token);
                }
            }

            // Reset states
            Host.RotateLeft(false);
            Host.RotateRight(false);
        }
    }
}
