using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Tests
{
    public class NameErrorTests
    {
        [Test]
        public void ShouldFixNameForMagnifyingGlassEnthusiast()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/8248"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/182814"), players);

            deck.Mainboard.FirstOrDefault(c => c.CardName == "Jacob Hauken, Inspector").Should().NotBeNull();
        }

        [Test]
        public void ShouldFixVoltaicVisionary()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/8248"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/182336"), players);

            deck.Mainboard.FirstOrDefault(c => c.CardName == "Voltaic Visionary").Should().NotBeNull();
        }

        [Test]
        public void ShouldFixNameStickerGoblin()
        {
            var players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/17900"));
            var deck = new MtgMeleeClient().GetDeck(new Uri("https://melee.gg/Decklist/View/329567"), players);

            deck.Mainboard.FirstOrDefault(c => c.CardName == "_____ Goblin").Should().NotBeNull();
        }
    }
}