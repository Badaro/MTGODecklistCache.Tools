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
    public class DeckNormalizerTests
    {
        [Test]
        public void ShouldReorderCards()
        {
            var input = new Deck()
            {
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Mishra's Bauble",         Count=3 },
                    new DeckItem(){ CardName="Dragon's Rage Channeler", Count=4 },
                    new DeckItem(){ CardName="Murktide Regent",         Count=4 },
                    new DeckItem(){ CardName="Delver of Secrets",       Count=1 },
                    new DeckItem(){ CardName="Brazen Borrower",         Count=1 },
                    new DeckItem(){ CardName="Counterbalance",          Count=2 },
                    new DeckItem(){ CardName="Force of Negation",       Count=1 },
                    new DeckItem(){ CardName="Daze",                    Count=3 }
                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Tropical Island",              Count=1 },
                    new DeckItem(){ CardName="End the Festivities",          Count=1 },
                    new DeckItem(){ CardName="Meltdown",                     Count=2 },
                    new DeckItem(){ CardName="Red Elemental Blast",          Count=1 },
                    new DeckItem(){ CardName="Pyroblast",                    Count=2 }
                }
            };

            var expected = new Deck()
            {
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Brazen Borrower",         Count=1 },
                    new DeckItem(){ CardName="Counterbalance",          Count=2 },
                    new DeckItem(){ CardName="Daze",                    Count=3 },
                    new DeckItem(){ CardName="Delver of Secrets",       Count=1 },
                    new DeckItem(){ CardName="Dragon's Rage Channeler", Count=4 },
                    new DeckItem(){ CardName="Force of Negation",       Count=1 },
                    new DeckItem(){ CardName="Mishra's Bauble",         Count=3 },
                    new DeckItem(){ CardName="Murktide Regent",         Count=4 }
                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="End the Festivities",          Count=1 },
                    new DeckItem(){ CardName="Meltdown",                     Count=2 },
                    new DeckItem(){ CardName="Pyroblast",                    Count=2 },
                    new DeckItem(){ CardName="Red Elemental Blast",          Count=1 },
                    new DeckItem(){ CardName="Tropical Island",              Count=1 }
                }
            };

            DeckNormalizer.Normalize(input).Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void ShouldCombineDuplicates()
        {
            var input = new Deck()
            {
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Mishra's Bauble",         Count=3 },
                    new DeckItem(){ CardName="Mishra's Bauble",         Count=1 },
                    new DeckItem(){ CardName="Dragon's Rage Channeler", Count=4 },
                    new DeckItem(){ CardName="Murktide Regent",         Count=4 },
                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Tropical Island",              Count=1 },
                    new DeckItem(){ CardName="End the Festivities",          Count=1 },
                    new DeckItem(){ CardName="End the Festivities",          Count=2 },
                    new DeckItem(){ CardName="Meltdown",                     Count=2 },
                }
            };

            var expected = new Deck()
            {
                Mainboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="Dragon's Rage Channeler", Count=4 },
                    new DeckItem(){ CardName="Mishra's Bauble",         Count=4 },
                    new DeckItem(){ CardName="Murktide Regent",         Count=4 }
                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem(){ CardName="End the Festivities",          Count=3 },
                    new DeckItem(){ CardName="Meltdown",                     Count=2 },
                    new DeckItem(){ CardName="Tropical Island",              Count=1 }
                }
            };

            DeckNormalizer.Normalize(input).Should().BeEquivalentTo(expected);
        }
    }
}
