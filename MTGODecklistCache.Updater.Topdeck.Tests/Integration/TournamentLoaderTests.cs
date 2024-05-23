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
    public class TournamentLoaderTests
    {
        private Tournament[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new TopdeckSource().GetTournaments(new DateTime(2024, 04, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 04, 30, 00, 00, 00));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            _testData.Length.Should().Be(7);
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            _testData.First().Should().BeEquivalentTo(new Tournament()
            {
                Name = "Emjati - Eternal - Modern",
                Date = new DateTime(2024, 04, 05, 06, 00, 00, DateTimeKind.Utc),
                Uri = new Uri("https://topdeck.gg/event/iCMd298218qbEqeGt5d7")
            }, o => o.Excluding(t => t.JsonFile));
        }
    }
}