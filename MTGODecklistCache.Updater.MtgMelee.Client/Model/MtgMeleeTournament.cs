using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeTournament : Tournament
    {
        public int? ID { get; set; }
        public string[]? Formats { get; set; }
        public int? MaxDecklists { get; set; }
        public int? DeckOffset { get; set; }
        public int? ExpectedDecks { get; set; }
        public MtgMeleeMissingDeckBehavior FixBehavior { get; set; }
        public string[] ExcludedRounds { get; set; }
    }

    public enum MtgMeleeMissingDeckBehavior
    {
        Skip,
        UseLast
    }
}
