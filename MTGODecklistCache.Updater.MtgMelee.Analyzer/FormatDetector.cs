using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    internal class FormatDetector
    {
        static readonly string[] _vintageCards = new string[] { "Black Lotus", "Mox Emerald", "Mox Jet", "Mox Sapphire", "Mox Ruby", "Mox Pearl" };
        static readonly string[] _legacyCards = new string[] { "Tundra", "Underground Sea", "Badlands", "Taiga", "Savannah", "Scrubland", "Volcanic Island", "Bayou", "Plateau", "Tropical Island" };
        static readonly string[] _modernCards = new string[] { "Flooded Strand", "Polluted Delta", "Bloodstained Mire", "Wooded Foothills", "Windswept Heath", "Marsh Flats", "Scalding Tarn", "Verdant Catacombs", "Arid Mesa", "Misty Rainforest" };
        static readonly string[] _pioneerCards1 = new string[] { "Hallowed Fountain", "Watery Grave", "Blood Crypt", "Stomping Ground", "Temple Garden", "Godless Shrine", "Overgrown Tomb", "Breeding Pool", "Steam Vents", "Sacred Foundry" };
        static readonly string[] _pioneerCards2 = new string[] { "Nykthos, Shrine to Nyx", "Savai Triome", "Indatha Triome", "Zagoth Triome", "Ketria Triome", "Raugrin Triome", "Spara's Headquarters", "Raffine's Tower", "Xander's Lounge", "Ziatora's Proving Ground", "Jetmir's Garden" };

        public static string Detect(MtgMeleeDeckInfo[] decks)
        {
            if (decks.Any(d => d.Mainboard.Any(c => _vintageCards.Contains(c.CardName)))) return "Vintage";
            if (decks.Any(d => d.Mainboard.Any(c => _legacyCards.Contains(c.CardName)))) return "Legacy";
            if (decks.Any(d => d.Mainboard.Any(c => _modernCards.Contains(c.CardName)))) return "Modern";
            if (decks.Any(d => d.Mainboard.Any(c => _pioneerCards1.Contains(c.CardName))) && decks.Any(d => d.Mainboard.Any(c => _pioneerCards2.Contains(c.CardName)))) return "Pioneer";
            return "Standard";
        }
    }
}
