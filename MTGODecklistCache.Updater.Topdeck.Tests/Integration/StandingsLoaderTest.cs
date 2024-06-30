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
    public class StandingsLoaderTest
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
        public void ShouldLoadStandings()
        {
            _testData.Standings.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void StandingsShouldHaveNames()
        {
            _testData.Standings.Should().AllSatisfy(s => s.Player.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void StandingsShouldHaveRanks()
        {
            _testData.Standings.Should().AllSatisfy(s => s.Rank.Should().BeGreaterThan(0));
        }

        [Test]
        public void StandingsShouldHaveWins()
        {
            _testData.Standings.Where(s => s.Wins > 0).Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsShouldHaveLosses()
        {
            _testData.Standings.Where(s => s.Losses > 0).Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsShouldHaveDraws()
        {
            _testData.Standings.Where(s => s.Draws > 0).Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsShouldHaveGameWinPercent()
        {
            _testData.Standings.Where(s => s.GWP > 0).Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsShouldHaveOpponentMatchWinPercent()
        {
            _testData.Standings.Where(s => s.OMWP > 0).Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsShouldHaveOpponentGameWinPercent()
        {
            _testData.Standings.Where(s => s.OGWP > 0).Count().Should().BeGreaterThan(0);
        }

    }
}