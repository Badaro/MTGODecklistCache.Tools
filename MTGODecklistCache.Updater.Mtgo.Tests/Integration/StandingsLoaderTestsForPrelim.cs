using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    class StandingsLoaderTestsForPrelim : StandingsLoaderTests
    {
        protected override Uri GetEventUri()
        {
            return new Uri("https://www.mtgo.com/decklist/modern-preliminary-2022-10-2512488091");
        }

        protected override Standing GetFirstStanding()
        {
            return new Standing()
            {
                Rank = 1,
                Player = "Aeolic",
                Points = 12,
                OMWP = 0.5625,
                GWP = 0.8000,
                OGWP = 0.5088,
                Wins = 4,
                Losses = 0,
                Draws = 0
            };

        }
        protected override int GetStandingsCount()
        {
            return 12;
        }
    }
}
