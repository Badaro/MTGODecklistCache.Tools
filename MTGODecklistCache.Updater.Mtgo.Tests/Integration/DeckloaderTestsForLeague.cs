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
            return 57;
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
                Player = "Frozon",
                Result = "5-0",
                AnchorUri = new Uri("https://www.mtgo.com/decklist/modern-league-2020-08-045236#deck_Frozon"),
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Arcbound Worker",      Count=4 },
                    new DeckItem(){ CardName="Llanowar Reborn",      Count=2 },
                    new DeckItem(){ CardName="Ancient Stirrings",    Count=4 },
                    new DeckItem(){ CardName="Steel Overseer",       Count=2 },
                    new DeckItem(){ CardName="Inkmoth Nexus",        Count=4 },
                    new DeckItem(){ CardName="Phyrexia's Core",      Count=1 },
                    new DeckItem(){ CardName="Darksteel Citadel",    Count=4 },
                    new DeckItem(){ CardName="Hardened Scales",      Count=4 },
                    new DeckItem(){ CardName="Hangarback Walker",    Count=4 },
                    new DeckItem(){ CardName="Animation Module",     Count=3 },
                    new DeckItem(){ CardName="Metallic Mimic",       Count=2 },
                    new DeckItem(){ CardName="Pendelhaven",          Count=1 },
                    new DeckItem(){ CardName="Scrapyard Recombiner", Count=2 },
                    new DeckItem(){ CardName="Nurturing Peatland",   Count=2 },
                    new DeckItem(){ CardName="Forest",               Count=8 },
                    new DeckItem(){ CardName="Crystalline Giant",    Count=2 },
                    new DeckItem(){ CardName="The Ozolith",          Count=3 },
                    new DeckItem(){ CardName="Arcbound Ravager",     Count=4 },
                    new DeckItem(){ CardName="Walking Ballista",     Count=4 },

                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Nature's Claim",      Count=2 },
                    new DeckItem(){ CardName="Torpor Orb",          Count=2 },
                    new DeckItem(){ CardName="Relic of Progenitus", Count=2 },
                    new DeckItem(){ CardName="Dismember",           Count=2 },
                    new DeckItem(){ CardName="Karn, Scion of Urza", Count=2 },
                    new DeckItem(){ CardName="Damping Sphere",      Count=2 },
                    new DeckItem(){ CardName="Veil of Summer",      Count=2 },
                    new DeckItem(){ CardName="Gemrazer",            Count=1 },
                },
            };
        }
    }
}
