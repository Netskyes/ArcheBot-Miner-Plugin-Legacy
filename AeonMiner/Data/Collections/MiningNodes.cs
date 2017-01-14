using System.Collections.Generic;
using System.Linq;

namespace AeonMiner.Data
{
    public static class MiningNodes
    {
        private static readonly List<Node> nodes = new List<Node>()
        {
            new Node(0, "Iron Vein", new uint[] { 3054 }),
            new Node(1, "Fortuna Vein", new uint[] { 3056, 17071, 17118, 17119, 17120, 17121, 17122, 17123, 17124, 17125, 17126, 22367 }),
            new Node(2, "Unidentified Vein", new uint[] { 34921, 35365 }),
            new Node(3, "Thiorium Vein", new uint[] { 8000405 }),
            new Node(4, "Starshard Vein", new uint[] { 13236, 13238, 13250, 13251, 13252, 13237, 13249 }),
            new Node(5, "Anya Vein", new uint[] { 16262 }),
            new Node(6, "Shinning Anya Vein", new uint[] { 17655 })
        };

        public static List<Node> GetAll()
        {
            return nodes;
        }

        public static Node GetById(int id)
        {
            return nodes.FirstOrDefault(n => n.Id == id);
        }

        public static uint[] GetPhasesById(int id)
        {
            return GetById(id).Phases;
        }

        public static bool Exists(uint phaseId)
        {
            return nodes.Any(n => n.Phases.Contains(phaseId));
        }


        public static class Veins
        {
            public static uint[] Iron => GetPhasesById(0);
            public static uint[] Fortuna => GetPhasesById(1);
            public static uint[] Unidentified => GetPhasesById(2);
            public static uint[] Thiorium => GetPhasesById(3);
            public static uint[] Starshard => GetPhasesById(4);
            public static uint[] Anya => GetPhasesById(5);
            public static uint[] ShinningAnya => GetPhasesById(6);
        }
    }
}
