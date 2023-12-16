using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    class StandingsLoaderTestsForLeague : StandingsLoaderTests
    {
        protected override Uri GetEventUri()
        {
            return new Uri("https://www.mtgo.com/decklist/modern-league-2020-08-045236");
        }

        protected override Standing GetFirstStanding()
        {
            return null;
        }

        protected override int GetStandingsCount()
        {
            return 0;
        }
    }
}
