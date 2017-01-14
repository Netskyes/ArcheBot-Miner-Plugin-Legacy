using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ArcheBot.Bot.Classes;

namespace AeonMiner
{
    public abstract class Helpers
    {
        private Host Host
        {
            get { return Host.Instance; }
        }

        public void Log(string text)
        {
            Host.Log(text);
        }

        public void Log(string text, Color color)
        {
            Host.Log(text, color);
        }

        public bool IsPatron()
        {
            return Host.getBuff(8000011) != null;
        }

        public bool InCombat()
        {
            return Host.getAggroMobsCount() > 0 || Host.me.inFight;
        }

        public bool CanUpgradeLevel(uint profId)
        {
            int[] levels = new int[] 
            {
                10000, 20000, 30000, 40000,
                50000, 70000, 90000, 110000, 130000, 150000
            };

            var points = Host.me.getActabilities().Find(a => a.db.id == profId)?.points ?? 0;

            return (levels.Contains(points));
        }

        public bool UpgradeLevel(uint profId)
        {
            return Host.me.getActabilities().Find(a => a.db.id == profId)?.IncreaseLevel() ?? false;
        }

        public bool IsBuffExists(uint buffId)
        {
            return Host.getBuff(buffId) != null;
        }

        public bool IsBuffExists(Creature obj, uint buffId)
        {
            return (Host.getBuff(obj, buffId) != null);
        }

        public bool IsAnyBuffExists(IEnumerable<uint> buffs)
        {
            return Host.me.getBuffs().Any(b => buffs.Contains(b.id));
        }

        public void CancelAnyBuffs(IEnumerable<uint> buffs)
        {
            var active = Host.me.getBuffs().Where(b => buffs.Contains(b.id));

            if (active.Count() > 0)
            {
                foreach (var b in active) b.CancelBuff();
            }
        }

        public bool IsItemExists(uint id)
        {
            return (Host.getInvItem(id) != null);
        }

        public bool IsItemExists(uint[] ids)
        {
            return (Host.getAllInvItems().Where(i => ids.Contains(i.id)).Count() > 0);
        }

        public bool IsInventoryFull()
        {
            return (Host.inventoryItemsCount() == Host.maxInventoryItemsCount());
        }

        public int InventoryFreeSpace()
        {
            return (Host.maxInventoryItemsCount() - Host.inventoryItemsCount());
        }

        public double AngleToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public double[] GetRayCast(double dist)
        {
            int angle = Host.angle(Host.me);

            double x = Host.me.X,
                   y = Host.me.Y,
                   z = Host.me.Z;


            x = x + dist * Math.Cos(AngleToRadians(angle));
            y = y + dist * Math.Sin(AngleToRadians(angle));
            z = Host.getZFromHeightMap(x, y);


            return new double[] { x, y, z };
        }

        public bool TurnToAngle(double nAngle)
        {
            double angle = Host.angle(Host.me);
            return Host.Turn(AngleToRadians((360 - angle) + nAngle), true);
        }

        public bool TurnToCoords(double x, double y)
        {
            int angle = Host.angle(Host.me, x, y);
            return Host.Turn(-(angle / 180 * Math.PI), true);
        }

        public bool IsCoordsBehind(double x, double y)
        {
            double ang = (Host.angle(Host.me, x, y) / 90);

            return (ang >= 1 && ang < 3);
        }

        public bool IsFacingAngle(double x, double y, int angle)
        {
            double ang = Host.angle(Host.me, x, y);

            return Math.Abs(((int)(ang / 180) * 360) - ang) < angle;
        }
    }
}
