using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using NuGet.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Tests.Integration
{
    public class TournamentLoaderTests
    {
        MtgMeleeTournamentInfo _tournament;

        [OneTimeSetUp]
        public void LoadTournament()
        {
            _tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/72980"));
        }

        [Test]
        public void ShouldLoadID()
        {
            _tournament.ID.Should().Be(72980);
        }

        [Test]
        public void ShouldLoadRounds()
        {
            _tournament.RoundIDs.Count().Should().Be(19);
        }

        [Test]
        public void ShouldHaveCorrectRoundData()
        {
            _tournament.RoundIDs.Last().Should().Be(242297);
        }

        [Test]
        public void ShouldLoadFormats()
        {
            _tournament.Formats.Should().BeEquivalentTo(new string[]
            {
                "Draft", 
                "Standard", 
                "Draft2"
            });
        }
    }
}
