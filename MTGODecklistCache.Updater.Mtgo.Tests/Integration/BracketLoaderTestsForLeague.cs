using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    class BracketLoaderTestsForLeague : BracketLoaderTests
    {
        protected override Round[] GetBracket()
        {
            return null;
        }

        protected override Uri GetEventUri()
        {
            return new Uri("https://www.mtgo.com/decklist/modern-league-2020-08-045236");
        }
    }
}
