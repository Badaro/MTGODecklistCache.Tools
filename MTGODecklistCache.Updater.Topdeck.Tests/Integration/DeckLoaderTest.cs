using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using System.Collections.Generic;
using System.IO;
using MTGODecklistCache.Updater.Topdeck;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class DeckLoaderTest
    {
        private CacheItem _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new TopdeckSource().GetTournamentDetails(new Tournament()
            {
                Name = "CCS Summer Showdown Modern 2k",
                Date = DateTimeOffset.FromUnixTimeSeconds(1717934400).UtcDateTime,
                Uri = new Uri("https://topdeck.gg/event/SrJAEZ8vbglVge29fG7l")
            });
        }

        [Test]
        public void ShouldLoadDecks()
        {
            _testData.Decks.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void DecksShouldHaveCards()
        {
            _testData.Decks.Should().AllSatisfy(d => d.Mainboard.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void DecksShouldHavePlayers()
        {
            _testData.Decks.Should().AllSatisfy(d => d.Player.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void DecksShouldHaveResults()
        {
            _testData.Decks.Should().AllSatisfy(d => d.Result.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void DecksShouldHaveAnchorUris()
        {
            _testData.Decks.Should().AllSatisfy(d => d.AnchorUri.Should().NotBeNull());
        }
    }
}