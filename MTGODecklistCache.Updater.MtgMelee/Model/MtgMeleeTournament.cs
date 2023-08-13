using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Model
{
    public class MtgMeleeTournament : Tournament
    {
        public int? DeckOffset { get; set; }
        public int? ExpectedDecks { get; set; }
        public int? PhaseOffset { get; set; }
        public MtgMeleeMissingDeckBehavior FixBehavior { get; set; }
        public string[] ExcludedRounds { get; set; }
    }

    public enum MtgMeleeMissingDeckBehavior
    {
        Skip,
        UseLast
    }
}
