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
                Name = "Emjati - Eternal - Modern",
                Date = new DateTime(2024, 04, 05, 06, 00, 00, DateTimeKind.Utc),
                Uri = new Uri("https://topdeck.gg/event/iCMd298218qbEqeGt5d7")
            });
        }

        [Test]
        public void ShouldLoadTournamentData()
        {
            Assert.Fail(); // To be implemented
        }
    }
}