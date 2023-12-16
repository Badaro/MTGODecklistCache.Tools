using FluentAssertions;
using MTGODecklistCache.Updater.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.Tools.Tests
{
    public class OrderNormalizerTests
    {
        [Test]
        public void ShouldReorderUsingBracket()
        {
            var decks = new Deck[]
            {
                new Deck() { Player = "Fifth",   Result = "1st Place" },
                new Deck() { Player = "Winner",  Result = "2nd Place" },
                new Deck() { Player = "Sixth",   Result = "3rd Place" },
                new Deck() { Player = "Seventh", Result = "4th Place" },
                new Deck() { Player = "Third",   Result = "5th Place" },
                new Deck() { Player = "Eighth",  Result = "6th Place" },
                new Deck() { Player = "Fourth",  Result = "7th Place" },
                new Deck() { Player = "Second",  Result = "8th Place" }
            };

            var standings = new Standing[]
            {

                new Standing() { Player= "Fifth",   Points=10 },
                new Standing() { Player= "Winner",  Points=9 },
                new Standing() { Player= "Sixth",   Points=8 },
                new Standing() { Player= "Seventh", Points=7 },
                new Standing() { Player= "Third",   Points=6 },
                new Standing() { Player= "Eighth",  Points=5 },
                new Standing() { Player= "Fourth",  Points=4 },
                new Standing() { Player= "Second",  Points=3 }
            };

            var bracket = new Round[]
            {
                new Round() { RoundName="Quarterfinals", Matches =  new RoundItem[]
                    {
                        new RoundItem() { Player1 = "Winner", Player2 = "Fifth",   Result = "2-0-0"},
                        new RoundItem() { Player1 = "Second", Player2 = "Sixth",   Result = "2-0-0"},
                        new RoundItem() { Player1 = "Third",  Player2 = "Seventh", Result = "2-0-0"},
                        new RoundItem() { Player1 = "Fourth", Player2 = "Eighth",  Result = "2-0-0"}
                    }
                },
                new Round() { RoundName="Semifinals", Matches =  new RoundItem[]
                    {
                        new RoundItem() { Player1 = "Winner", Player2 = "Third",  Result = "2-0-0"},
                        new RoundItem() { Player1 = "Second", Player2 = "Fourth", Result = "2-0-0"}
                    }
                },
                new Round() { RoundName="Finals", Matches = new RoundItem[]
                    {
                        new RoundItem() { Player1 = "Winner", Player2 = "Second", Result = "2-0-0"}
                    }
                }
            };

            OrderNormalizer.ReorderDecks(decks, standings, bracket, true).Should().BeEquivalentTo(new Deck[]
            {
                new Deck() { Player = "Winner",  Result = "1st Place" },
                new Deck() { Player = "Second",  Result = "2nd Place" },
                new Deck() { Player = "Third",   Result = "3rd Place" },
                new Deck() { Player = "Fourth",  Result = "4th Place" },
                new Deck() { Player = "Fifth",   Result = "5th Place" },
                new Deck() { Player = "Sixth",   Result = "6th Place" },
                new Deck() { Player = "Seventh", Result = "7th Place" },
                new Deck() { Player = "Eighth",  Result = "8th Place" }
            }, options => options.WithStrictOrdering());
        }

    }
}
