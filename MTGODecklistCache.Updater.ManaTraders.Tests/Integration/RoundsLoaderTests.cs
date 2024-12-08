using FluentAssertions;
using MTGODecklistCache.Updater.ManaTraders;
using MTGODecklistCache.Updater.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MagicGG.Tests
{
    public class RoundsLoaderTests
    {
        private Round[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new ManaTradersSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.manatraders.com/tournaments/30/"),
                Date = new DateTime(2022, 08, 31, 00, 00, 00, DateTimeKind.Utc)
            }).Rounds;
        }

        [Test]
        public void RoundCountIsCorrect()
        {
            _testData.Length.Should().Be(3);
        }


        [Test]
        public void RoundDataIsCorrect()
        {
            Round testRound = _testData.First();
            testRound.RoundName.Should().Be("Quarterfinals");
            testRound.Matches.First().Should().BeEquivalentTo(new RoundItem()
            {
                Player1 = "zuri1988",
                Player2 = "Fink64",
                Result = "2-1-0"
            });
        }
    }
}
