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
            _testData = new TopdeckSource().GetTournaments(new DateTime(2023, 09, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2023, 09, 07, 00, 00, 00));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            _testData.Length.Should().Be(12);
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            _testData.First().Should().BeEquivalentTo(new Tournament()
            {
                Name = "Berlin Double Up Legacy VIII im Brettspielplatz 07.09.23",
                Date = new DateTime(2023, 09, 07, 17, 15, 00, DateTimeKind.Utc),
                Uri = new Uri("https://melee.gg/Tournament/View/18285")
            }, o => o.Excluding(t => t.JsonFile));
        }
    }
}