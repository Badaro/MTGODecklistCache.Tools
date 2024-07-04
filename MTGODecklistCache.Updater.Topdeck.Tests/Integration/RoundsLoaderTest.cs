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
    public class RoundsLoaderTest
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
        public void ShouldLoadRounds()
        {
            _testData.Rounds.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void RoundsShouldHaveNames()
        {
            _testData.Rounds.Should().AllSatisfy(r => r.RoundName.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void RoundsShouldHaveMatches()
        {
            _testData.Rounds.Should().AllSatisfy(r => r.Matches.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void MatchesShouldHavePlayers()
        {
            _testData.Rounds.SelectMany(r => r.Matches).Should().AllSatisfy(m => m.Player1.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void MatchesShouldHaveResults()
        {
            _testData.Rounds.SelectMany(r => r.Matches).Should().AllSatisfy(m => m.Result.Should().NotBeNullOrEmpty());
        }
    }
}