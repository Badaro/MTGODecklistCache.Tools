using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using System.Collections.Generic;
using System.IO;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class TournamentLoaderTests
    {
        private Tournament[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgMeleeSource().GetTournaments(new DateTime(2023, 09, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2023, 09, 07, 00, 00, 00));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            throw new NotImplementedException();
        }
    }
}