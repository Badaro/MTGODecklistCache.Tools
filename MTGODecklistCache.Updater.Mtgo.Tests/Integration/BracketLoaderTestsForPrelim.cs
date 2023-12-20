using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    class BracketLoaderTestsForPrelim : BracketLoaderTests
    {
        protected override Uri GetEventUri()
        {
            return new Uri("https://www.mtgo.com/decklist/modern-preliminary-2022-10-2512488091");
        }

        protected override Round[] GetBracket()
        {
            return null;
        }
    }
}
