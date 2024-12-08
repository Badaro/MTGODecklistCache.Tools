using FluentAssertions;
using MTGODecklistCache.Updater.MtgMelee;
using MTGODecklistCache.Updater.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class DeckLoaderTests
    {
        private Deck[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgMeleeSource().GetTournamentDetails(new MtgMeleeTournament()
            {
                Uri = new Uri("https://melee.gg/Tournament/View/12867"),
                Date = new DateTime(2022, 11, 19, 00, 00, 00, DateTimeKind.Utc)
            }).Decks;
        }

        [Test]
        public void DeckCountIsCorrect()
        {
            _testData.Length.Should().Be(6);
        }

        [Test]
        public void DecksDontHaveDate()
        {
            foreach (var deck in _testData) deck.Date.Should().BeNull();
        }

        [Test]
        public void DecksHavePlayers()
        {
            foreach (var deck in _testData) deck.Player.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void DecksHaveMainboards()
        {
            foreach (var deck in _testData) deck.Mainboard.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public void DecksHaveSideboards()
        {
            foreach (var deck in _testData) deck.Sideboard.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public void DecksHaveValidMainboards()
        {
            foreach (var deck in _testData) deck.Mainboard.Sum(i => i.Count).Should().BeGreaterOrEqualTo(60); ;
        }

        [Test]
        public void DecksHaveValidSideboards()
        {
            foreach (var deck in _testData) deck.Sideboard.Sum(i => i.Count).Should().BeLessOrEqualTo(15);
        }

        [Test]
        public void DeckDataIsCorrect()
        {
            Deck testDeck = _testData.Skip(3).First();
            testDeck.Should().BeEquivalentTo(new Deck()
            {
                Player = "SB36",
                AnchorUri = new Uri("https://melee.gg/Decklist/View/257079"),
                Date = null,
                Result = "4th Place",
                Mainboard = new DeckItem[]
                {
                    new DeckItem() { Count=4, CardName= "The Mightstone and Weakstone" },
                    new DeckItem() { Count=3, CardName= "Urza, Lord Protector" },
                    new DeckItem() { Count=4, CardName= "Bonecrusher Giant" },
                    new DeckItem() { Count=4, CardName= "Omnath, Locus of Creation" },
                    new DeckItem() { Count=4, CardName= "Fable of the Mirror-Breaker" },
                    new DeckItem() { Count=1, CardName= "Temporary Lockdown" },
                    new DeckItem() { Count=4, CardName= "Leyline Binding" },
                    new DeckItem() { Count=4, CardName= "Fires of Invention" },
                    new DeckItem() { Count=1, CardName= "Snow-Covered Forest" },
                    new DeckItem() { Count=1, CardName= "Stomping Ground" },
                    new DeckItem() { Count=1, CardName= "Snow-Covered Mountain" },
                    new DeckItem() { Count=1, CardName= "Boseiju, Who Endures" },
                    new DeckItem() { Count=1, CardName= "Temple Garden" },
                    new DeckItem() { Count=1, CardName= "Breeding Pool" },
                    new DeckItem() { Count=1, CardName= "Steam Vents" },
                    new DeckItem() { Count=2, CardName= "Ketria Triome" },
                    new DeckItem() { Count=1, CardName= "Snow-Covered Island" },
                    new DeckItem() { Count=1, CardName= "Snow-Covered Plains" },
                    new DeckItem() { Count=2, CardName= "Hallowed Fountain" },
                    new DeckItem() { Count=1, CardName= "Spara's Headquarters" },
                    new DeckItem() { Count=1, CardName= "Ziatora's Proving Ground" },
                    new DeckItem() { Count=1, CardName= "Indatha Triome" },
                    new DeckItem() { Count=2, CardName= "Sacred Foundry" },
                    new DeckItem() { Count=2, CardName= "Jetmir's Garden" },
                    new DeckItem() { Count=3, CardName= "Raugrin Triome" },
                    new DeckItem() { Count=4, CardName= "Fabled Passage" },
                    new DeckItem() { Count=1, CardName= "Colossal Skyturtle" },
                    new DeckItem() { Count=1, CardName= "Otawara, Soaring City" },
                    new DeckItem() { Count=1, CardName= "Touch the Spirit Realm" },
                    new DeckItem() { Count=2, CardName= "Supreme Verdict" }
                },
                Sideboard = new DeckItem[]
                {
                    new DeckItem() { Count=1, CardName= "Keruga, the Macrosage" },
                    new DeckItem() { Count=1, CardName= "Chandra, Awakened Inferno" },
                    new DeckItem() { Count=4, CardName= "Leyline of the Void" },
                    new DeckItem() { Count=3, CardName= "Mystical Dispute" },
                    new DeckItem() { Count=1, CardName= "Supreme Verdict" },
                    new DeckItem() { Count=3, CardName= "Knight of Autumn" },
                    new DeckItem() { Count=1, CardName= "Temporary Lockdown" },
                    new DeckItem() { Count=1, CardName= "Twinshot Sniper" }
                },
            });
        }
    }
}
