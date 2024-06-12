using FluentAssertions;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Tests.Integration
{
    public class DeckLoaderTests
    {
        MtgMeleeDeckInfo _deck;
        MtgMeleeDeckInfo _deckNoRounds;

        [OneTimeSetUp]
        public void LoadDeck()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/16429"));
            _deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/315233"), players);
            _deckNoRounds = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/315233"), players, true);
        }

        [Test]
        public void ShouldLoadMainboardCards()
        {
            _deck.Mainboard.Should().BeEquivalentTo(new DeckItem[]
            {
                new DeckItem() { Count=4, CardName= "Ancient Stirrings" },
                new DeckItem() { Count=4, CardName= "Chromatic Sphere" },
                new DeckItem() { Count=1, CardName= "Chromatic Star" },
                new DeckItem() { Count=4, CardName= "Expedition Map" },
                new DeckItem() { Count=3, CardName= "Relic of Progenitus" },
                new DeckItem() { Count=3, CardName= "Dismember" },
                new DeckItem() { Count=1, CardName= "Talisman of Resilience" },
                new DeckItem() { Count=3, CardName= "Sylvan Scrying" },
                new DeckItem() { Count=4, CardName= "Oblivion Stone" },
                new DeckItem() { Count=4, CardName= "Karn, the Great Creator" },
                new DeckItem() { Count=4, CardName= "The One Ring" },
                new DeckItem() { Count=2, CardName= "Wurmcoil Engine" },
                new DeckItem() { Count=1, CardName= "Karn Liberated" },
                new DeckItem() { Count=2, CardName= "Ulamog, the Ceaseless Hunger" },
                new DeckItem() { Count=1, CardName= "Warping Wail" },
                new DeckItem() { Count=1, CardName= "Boseiju, Who Endures" },
                new DeckItem() { Count=3, CardName= "Forest" },
                new DeckItem() { Count=4, CardName= "Urza's Mine" },
                new DeckItem() { Count=4, CardName= "Urza's Power Plant" },
                new DeckItem() { Count=4, CardName= "Urza's Tower" },
                new DeckItem() { Count=2, CardName= "Urza's Saga" },
                new DeckItem() { Count=1, CardName= "Walking Ballista" }
            });
        }

        [Test]
        public void ShouldLoadSideboardCards()
        {
            _deck.Sideboard.Should().BeEquivalentTo(new DeckItem[]
            {
                new DeckItem() { Count=2, CardName= "Haywire Mite" },
                new DeckItem() { Count=1, CardName= "Cityscape Leveler" },
                new DeckItem() { Count=1, CardName= "Tormod's Crypt" },
                new DeckItem() { Count=1, CardName= "Pithing Needle" },
                new DeckItem() { Count=1, CardName= "Liquimetal Coating" },
                new DeckItem() { Count=1, CardName= "The Stone Brain" },
                new DeckItem() { Count=1, CardName= "Ensnaring Bridge" },
                new DeckItem() { Count=1, CardName= "Sundering Titan" },
                new DeckItem() { Count=1, CardName= "Engineered Explosives" },
                new DeckItem() { Count=1, CardName= "Walking Ballista" },
                new DeckItem() { Count=1, CardName= "Wurmcoil Engine" },
                new DeckItem() { Count=1, CardName= "Chalice of the Void" },
                new DeckItem() { Count=1, CardName= "Grafdigger's Cage" },
                new DeckItem() { Count=1, CardName= "Phyrexian Metamorph" }
            });
        }

        [Test]
        public void ShouldLoadRounds()
        {
            _deck.Rounds.Should().BeEquivalentTo(new MtgMeleeRoundInfo[]
            {
                new MtgMeleeRoundInfo() { RoundName = "Round 1",  Match = new RoundItem() { Player1="CHENG YU CHANG",   Player2="Javier Dominguez",    Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 2",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Guillaume Wafo-Tapa", Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 3",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Kelvin Hoon",         Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 4",  Match = new RoundItem() { Player1="Marco Del Pivo",   Player2="Javier Dominguez",    Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 5",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Lorenzo Terlizzi",    Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 6",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Anthony Lee",         Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 7",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Edgar Magalhaes",     Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 8",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="David Olsen",         Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 9",  Match = new RoundItem() { Player1="Javier Dominguez", Player2="Matthew Anderson",    Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 10", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Ben Jones",           Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 11", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Yiren Jiang",         Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 12", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Marei Okamura",       Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 13", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Simon Nielsen",       Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 14", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Kazune Kosaka",       Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 15", Match = new RoundItem() { Player1="Javier Dominguez", Player2="Christian Calcano",   Result="2-1-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 16", Match = new RoundItem() { Player1="Javier Dominguez", Player2="-",                   Result="2-0-0" }},
                new MtgMeleeRoundInfo() { RoundName = "Round 17", Match = new RoundItem() { Player1="Dominic Harvey",   Player2="Javier Dominguez",    Result="3-1-0" }}
            });
        }

        [Test]
        public void ShouldIncludeUri()
        {
            _deck.DeckUri.Should().Be(new Uri("https://melee.gg/Decklist/View/315233"));
        }

        [Test]
        public void ShouldIncludeFormat()
        {
            _deck.Format.Should().Be("Modern");
        }

        [Test]
        public void ShouldRespectFlagToSkipRounds()
        {
            _deckNoRounds.Rounds.Should().BeNull();
        }

        [Test]
        public void ShouldNotBreakOnDecksMissingRounds()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/21"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/96"), players);
            deck.Rounds.Should().BeNull();
        }

        [Test]
        public void ShouldNotBreakOnPlayerNamesWithBrackets()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/7891"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/170318"), players);
            deck.Rounds.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBreakOnPlayerNamesWithBracketsGettingABye()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/14720"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/284652"), players);
            deck.Rounds.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBreakOnFormatExceptionErrors()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/15300"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/292670"), players);
            deck.Rounds.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBreakOnDoubleForfeitMessage()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/65803"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/399414"), players);
            deck.Rounds.Should().NotBeNull();
        }
    }
}
