using System.Collections.Generic;
using System.Linq;

namespace AeonMiner.Data
{
    public static class Mounts
    {
        private static readonly List<Mount> mounts = new List<Mount>()
        {
            new Mount(8001139, "30-Day Black Reindeer"),
            new Mount(8001148, "30-Day Celestial Kitsu"),
            new Mount(8001143, "30-Day Celestial Pegasus"),
            new Mount(8001144, "30-Day Lord Cottontail"),
            new Mount(8001141, "30-Day Mirage Bjorne"),
            new Mount(8000037, "30-Day Polaris Bjorne"),
            new Mount(8001138, "30-Day Racing Zebra"),
            new Mount(8001149, "30-Day Rajani"),
            new Mount(8001146, "30-Day Soulmare"),
            new Mount(8001140, "30-Day White Reindeer"),
            new Mount(8163, "Alabaster Lilyut Horse"),
            new Mount(30774, "Albino Yata"),
            new Mount(37136, "Andelph Patrol Mech"),
            new Mount(36616, "Bestcargot Racing Snail"),
            new Mount(28138, "Black Arrow"),
            new Mount(8158, "Black Lilyut Horse"),
            new Mount(37148, "Black Reindeer"),
            new Mount(26629, "Black Yata"),
            new Mount(19516, "Blacktail Leomorph"),
            new Mount(28512, "Bloodclaw Ursun"),
            new Mount(8159, "Brown Lilyut Horse"),
            new Mount(36888, "Brown Reindeer"),
            new Mount(19361, "Browntail Leomorph"),
            new Mount(8161, "Buckskin Lilyut Horse"),
            new Mount(25222, "Calico Sabrefang"),
            new Mount(36794, "Candy-fueled Bestcargot"),
            new Mount(36793, "Candy-fueled Fastropod"),
            new Mount(39423, "Cardinal Comet"),
            new Mount(35302, "Carrot Wings"),
            new Mount(39568, "Celestial Kitsu"),
            new Mount(28736, "Celestial Pegasus"),
            new Mount(39564, "Cloudstrike Panther"),
            new Mount(15561, "Coalmane Snowlion"),
            new Mount(32566, "Crimson Lightning"),
            new Mount(8162, "Dappled Lilyut Horse"),
            new Mount(40377, "Dread Steed"),
            new Mount(8000086, "Drumstick"),
            new Mount(14879, "Earthen Roar"),
            new Mount(28753, "Ebonfur Bjorne"),
            new Mount(30183, "Elephant"),
            new Mount(26427, "Emberwild Charger"),
            new Mount(36615, "Fastropod Racing Snail"),
            new Mount(28420, "Fleetpaw Bjorne"),
            new Mount(39706, "Gallant Blacktail Leomorph"),
            new Mount(39708, "Gallant Brown Lilyut Horse"),
            new Mount(39707, "Gallant Browntail Leomorph"),
            new Mount(39710, "Gallant Buckskin Lilyut Horse"),
            new Mount(39702, "Gallant Coalmane Snowlion"),
            new Mount(39709, "Gallant Gray Lilyut Horse"),
            new Mount(39711, "Gallant Green Elk"),
            new Mount(39704, "Gallant Sandmane Snowlion"),
            new Mount(39701, "Gallant Snowmane Snowlion"),
            new Mount(39712, "Gallant Violet Elk"),
            new Mount(39713, "Gallant White Elk"),
            new Mount(39705, "Gallant Whitetail Leomorph"),
            new Mount(40930, "Golden Wyvern"),
            new Mount(8160, "Gray Lilyut Horse"),
            new Mount(23632, "Green Elk"),
            new Mount(41081, "Grumpytree"),
            new Mount(8001184, "Gweonid Vine Giant"),
            new Mount(27264, "Heir's Horse"),
            new Mount(30043, "Hooftiger Boar"),
            new Mount(40001, "Kamari"),
            new Mount(38771, "Lady Fluffsworth"),
            new Mount(34981, "Lavaspark"),
            new Mount(4177, "Lilyut Horse"),
            new Mount(38768, "Lord Cottontail"),
            new Mount(29494, "Mirage Bjorne"),
            new Mount(29488, "Mirage Elk"),
            new Mount(27160, "Mirage Emberwild Charger"),
            new Mount(29491, "Mirage Leomorph"),
            new Mount(39017, "Mirage Lilyut Horse"),
            new Mount(39018, "Mirage Snowlion"),
            new Mount(8000527, "Mirage Steel Lightning"),
            new Mount(37507, "Moonfeather Griffin"),
            new Mount(8000967, "Narayana Squire"),
            new Mount(37140, "Nocturne Griffin"),
            new Mount(26630, "Palomino Yata"),
            new Mount(28752, "Polaris Bjorne"),
            new Mount(27952, "Racing Zebra"),
            new Mount(40003, "Rajani"),
            new Mount(37141, "Royal Griffin 1"),
            new Mount(8000245, "Royal Griffin 2"),
            new Mount(4941, "Sandmane Snowlion"),
            new Mount(25217, "Seal-Point Sabrefang"),
            new Mount(40002, "Shayeera"),
            new Mount(8000606, "Sheepie"),
            new Mount(36591, "Shellraiser Racing Snail"),
            new Mount(38770, "Sir Hoppington"),
            new Mount(36607, "Snail 2"),
            new Mount(15560, "Snowmane Snowlion"),
            new Mount(30042, "Snowrend Boar"),
            new Mount(39022, "Soulmare"),
            new Mount(39364, "Steel Lightning"),
            new Mount(14878, "Stormdarter"),
            new Mount(8001042, "Stormrose"),
            new Mount(30209, "Stormwing Pegasus"),
            new Mount(25219, "Striped Sabrefang"),
            new Mount(26631, "Striped Yata"),
            new Mount(25120, "Tabby Sabrefang"),
            new Mount(28087, "Thunder Dash"),
            new Mount(8000510, "Untamed Royal Griffin"),
            new Mount(23661, "Violet Elk"),
            new Mount(23662, "White Elk"),
            new Mount(37147, "White Reindeer 1"),
            new Mount(8000124, "White Reindeer 2"),
            new Mount(8001685, "White Reindeer 3"),
            new Mount(19517, "Whitetail Leomorph"),
            new Mount(15061, "Yata"),
            new Mount(26052, "Zebra"),
            new Mount(41740, "Redscale Pangolin"),
            new Mount(30041, "Nightslaver Boar"),
            new Mount(8001710, "Iron Eviscerator")
        };


        public static int Count
        {
            get { return mounts.Count; }
        }

        public static IEnumerable<Mount> GetAll()
        {
            foreach (var mount in mounts)
            {
                yield return mount;
            }
        }

        public static Mount GetById(uint id)
        {
            return GetAll().Where(s => s.Id == id).First();
        }

        public static Mount GetByName(string name)
        {
            return GetAll().Where(s => s.Name == name).First();
        }
    }
}
