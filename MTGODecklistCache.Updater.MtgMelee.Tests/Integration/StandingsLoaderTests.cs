using FluentAssertions;
using MTGODecklistCache.Updater.MtgMelee;
using MTGODecklistCache.Updater.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.MtgMelee.Analyzer;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class StandingsLoaderTests
    {
        private Standing[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgMeleeSource().GetTournamentDetails(new MtgMeleeTournament()
            {
                Uri = new Uri("https://melee.gg/Tournament/View/86543"),
                Date = new DateTime(2024, 04, 24, 00, 00, 00, DateTimeKind.Utc)
            }).Standings;
        }

        [Test]
        public void StandingsCountIsCorrect()
        {
            _testData.Length.Should().Be(9);
        }

        [Test]
        public void StandingsHavePlayers()
        {
            foreach (var standing in _testData) standing.Player.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void StandingsHaveRank()
        {
            foreach (var standing in _testData) standing.Rank.Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsHavePoints()
        {
            foreach (var standing in _testData.Take(8)) standing.Points.Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsHaveOMWP()
        {
            foreach (var standing in _testData) standing.OMWP.Should().BeGreaterThan(0);
        }

        [Test]
        public void DecksHaveGWP()
        {
            foreach (var standing in _testData) standing.GWP.Should().BeGreaterThan(0);
        }

        [Test]
        public void DecksHaveOGWP()
        {
            foreach (var standing in _testData) standing.OGWP.Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingDataIsCorrect()
        {
            Standing testStanding = _testData.Skip(3).First();
            testStanding.Should().BeEquivalentTo(new Standing()
            {
                Rank = 4,
                Player = "Elston",
                Points = 6,
                OMWP = 0.75,
                GWP = 0.44444444,
                OGWP = 0.75661376,
                Wins = 2,
                Losses = 2,
                Draws = 0
            });
        }
    }
}
