﻿using FluentAssertions;
using MTGODecklistCache.Updater.Mtgo;
using MTGODecklistCache.Updater.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    abstract class StandingsLoaderTests
    {
        private Standing[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = this.GetEventUri()
            }).Standings;
        }

        [Test]
        public void StandingsCountIsCorrect()
        {
            if (_testData != null)
            {
                _testData.Length.Should().Be(this.GetStandingsCount());
            }
            else
            {
                this.GetStandingsCount().Should().Be(0);
            }
        }

        [Test]
        public void StandingsHavePlayers()
        {
            if (_testData != null) foreach (var standing in _testData) standing.Player.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void StandingsHaveRank()
        {
            if (_testData != null) foreach (var standing in _testData) standing.Rank.Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsHavePoints()
        {
            if (_testData != null) _testData.Max(s => s.Points).Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingsHaveOMWP()
        {
            if (_testData != null) _testData.Max(s => s.OMWP).Should().BeGreaterThan(0);
        }

        [Test]
        public void DecksHaveGWP()
        {
            if (_testData != null) _testData.Max(s => s.GWP).Should().BeGreaterThan(0);
        }

        [Test]
        public void DecksHaveOGWP()
        {
            if (_testData != null) _testData.Max(s => s.OGWP).Should().BeGreaterThan(0);
        }

        [Test]
        public void StandingDataIsCorrect()
        {
            if (_testData != null)
            {
                Standing testStanding = _testData.FirstOrDefault();
                testStanding.Should().BeEquivalentTo(this.GetFirstStanding());
            }
        }

        protected abstract Uri GetEventUri();
        protected abstract int GetStandingsCount();
        protected abstract Standing GetFirstStanding();
    }
}
