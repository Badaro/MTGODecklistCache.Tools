using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    internal class MtgMeleeAnalyzerSettings
    {
        public static readonly int MinimumPlayers = 16;
        public static readonly double MininumPercentageOfDecks = 0.5;
        public static readonly string[] ValidFormats = new string[]
        {
            "Standard",
            "Modern",
            "Pioneer",
            "Legacy",
            "Vintage",
            "Pauper"
        };
    }
}
