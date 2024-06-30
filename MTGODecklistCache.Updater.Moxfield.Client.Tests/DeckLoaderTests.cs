using FluentAssertions;
using MTGODecklistCache.Updater.Model;

namespace MTGODecklistCache.Updater.Moxfield.Client.Tests
{
    public class Tests
    {
        private Deck _testDeck;

        [OneTimeSetUp]
        public void DownloadDeck()
        {
            _testDeck = new MoxfieldClient().GetDeck("UaSpePO8U0ODB9DpOFpc_Q");
        }

        [Test]
        public void ShouldContainValidData()
        {
            _testDeck.Should().BeEquivalentTo(new Deck
            {
                AnchorUri = new Uri("https://www.moxfield.com/decks/UaSpePO8U0ODB9DpOFpc_Q"),
                Mainboard = new DeckItem[]
                {
                    new DeckItem{ Count=4, CardName="Arclight Phoenix" },
                    new DeckItem{ Count=2, CardName="Chart a Course" },
                    new DeckItem{ Count=4, CardName="Consider" },
                    new DeckItem{ Count=4, CardName="Fiery Impulse" },
                    new DeckItem{ Count=2, CardName="Galvanic Iteration" },
                    new DeckItem{ Count=2, CardName="Hall of Storm Giants" },
                    new DeckItem{ Count=2, CardName="Island" },
                    new DeckItem{ Count=4, CardName="Ledger Shredder" },
                    new DeckItem{ Count=3, CardName="Lightning Axe" },
                    new DeckItem{ Count=4, CardName="Opt" },
                    new DeckItem{ Count=1, CardName="Otawara, Soaring City" },
                    new DeckItem{ Count=4, CardName="Pieces of the Puzzle" },
                    new DeckItem{ Count=4, CardName="Riverglide Pathway" },
                    new DeckItem{ Count=2, CardName="Spell Pierce" },
                    new DeckItem{ Count=2, CardName="Spikefield Hazard" },
                    new DeckItem{ Count=4, CardName="Spirebluff Canal" },
                    new DeckItem{ Count=4, CardName="Steam Vents" },
                    new DeckItem{ Count=2, CardName="Stormcarved Coast" },
                    new DeckItem{ Count=2, CardName="Temporal Trespass" },
                    new DeckItem{ Count=1, CardName="Thing in the Ice" },
                    new DeckItem{ Count=3, CardName="Treasure Cruise" }
                },
                Sideboard= new DeckItem[]
                {
                    new DeckItem{ Count=2, CardName="Abrade" },
                    new DeckItem{ Count=2, CardName="Aether Gust" },
                    new DeckItem{ Count=2, CardName="Brotherhood's End" },
                    new DeckItem{ Count=2, CardName="Crackling Drake" },
                    new DeckItem{ Count=2, CardName="Disdainful Stroke" },
                    new DeckItem{ Count=2, CardName="Mystical Dispute" },
                    new DeckItem{ Count=3, CardName="Thing in the Ice" }
                }
            });
        }
    }
}