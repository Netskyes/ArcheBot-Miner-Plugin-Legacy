using System.Collections.Generic;

namespace AeonMiner.Data
{
    public static class Translations
    {
        private static readonly Dictionary<string, string> russian = new Dictionary<string, string>()
        {
            { "Iron Ore", "Железная руда" },
            { "Pure Iron Ore", "Чистая железная руда" },
            { "Copper Ore", "Медная руда" },
            { "Pure Copper Ore", "Чистая медная руда" },
            { "Silver Ore", "Серебряный самородок" },
            { "Pure Silver Ore", "Чистый серебряный самородок" },
            { "Gold Ore", "Золотой самородок" },
            { "Pure Gold Ore", "Чистый золотой самородок" },
            { "Archeum Ore", "Самородок акхиума" },
            { "Pure Archeum Ore", "Чистый самородок акхиума" },
            { "Anya Pebble", "Анадиевая руда" },
            { "Clear Unidentified Ore", "Блестящий самородок" },
            { "Lucid Unidentified Ore", "Переливающийся самородок" },
            { "Vivid Unidentified Ore", "Яркий самородок" },
            { "Raw Stone", "Камень" },
            { "Diamond", "Алмаз" },
            { "Amethyst", "Горный хрусталь" },
            { "Emerald", "Изумруд" },
            { "Ruby", "Рубин" },
            { "Sapphire", "Сапфир" },
            { "Topaz", "Топаз" }
        };

        public static string ToRussian(this string word)
        {
            return (russian.ContainsKey(word)) ? russian[word] : word; 
        }
    }
}
