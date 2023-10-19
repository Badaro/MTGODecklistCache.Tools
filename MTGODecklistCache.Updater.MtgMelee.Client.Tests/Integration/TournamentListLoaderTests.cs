using FluentAssertions;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.MtgMelee.Tests.Integration
{
    internal class TournamentListLoaderTests
    {
        MtgMeleeTournamentInfo[] _tournamentResults;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _tournamentResults = new MtgMeleeClient().GetTournaments(new DateTime(2023, 09, 01), new DateTime(2023, 09, 08));
        }

        [Test]
        public void ShouldHaveCorrectCount()
        {
            _tournamentResults.Length.Should().Be(21);
        }

        [Test]
        public void ShouldHaveCorrectResults()
        {
            _tournamentResults.First().Should().BeEquivalentTo(new MtgMeleeTournamentInfo()
            {
                ID = 25056,
                Name = "LA LIGA 5CH",
                Organizer = "Five Color Hub",
                Date = new DateTime(2023, 09, 08, 00, 00, 00, DateTimeKind.Utc),
                Formats = new string[] { "Explorer" }
            });
        }
    }
}
