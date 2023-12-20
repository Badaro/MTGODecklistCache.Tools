using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    class DeckloaderTestsForLeague : DeckLoaderTests
    {
        protected override Uri GetEventUri()
        {
            return new Uri("https://www.mtgo.com/decklist/modern-league-2020-08-045236");
        }

        protected override int GetDeckCount()
        {
            return 729;
        }

        protected override DateTime GetEventDate()
        {
            return new DateTime(2020, 08, 04, 00, 00, 00, DateTimeKind.Utc);
        }

        protected override DateTime? GetDeckDate()
        {
            return new DateTime(2020, 08, 04, 00, 00, 00, DateTimeKind.Utc);
        }

        protected override Deck GetFirstDeck()
        {
            return new Deck()
            {
                Date = new DateTime(2020, 08, 04, 00, 00, 00, DateTimeKind.Utc),
                Player = "LalauWBA",
                Result = "4-1",
                AnchorUri = new Uri("https://www.mtgo.com/decklist/modern-league-2020-08-045236"),
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Mountain",             Count=16 },
                    new DeckItem(){ CardName="Lightning Bolt",       Count=4 },
                    new DeckItem(){ CardName="Monastery Swiftspear", Count=4 },
                    new DeckItem(){ CardName="Soul-Scar Mage",       Count=4 },
                    new DeckItem(){ CardName="Ghitu Lavarunner",     Count=4 },
                    new DeckItem(){ CardName="Wizard's Lightning",   Count=4 },
                    new DeckItem(){ CardName="Light Up the Stage",   Count=4 },
                    new DeckItem(){ CardName="Firebolt",             Count=4 },
                    new DeckItem(){ CardName="Lava Dart",            Count=4 },
                    new DeckItem(){ CardName="Fiery Islet",          Count=2 },
                    new DeckItem(){ CardName="Sunbaked Canyon",      Count=2 },
                    new DeckItem(){ CardName="Bonecrusher Giant",    Count=4 },
                    new DeckItem(){ CardName="Heartfire Immolator",  Count=4 },

                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Relic of Progenitus",      Count=4 },
                    new DeckItem(){ CardName="Shrine of Burning Rage",   Count=1 },
                    new DeckItem(){ CardName="Blood Moon",               Count=3 },
                    new DeckItem(){ CardName="Kozilek's Return",         Count=2 },
                    new DeckItem(){ CardName="Abrade",                   Count=4 },
                    new DeckItem(){ CardName="Jegantha, the Wellspring", Count=1 },
                },
            };
        }
    }
}
