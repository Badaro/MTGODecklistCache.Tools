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
        MtgMeleeListTournamentInfo[] _tournamentResults;
        MtgMeleeListTournamentInfo[] _tournamentResultsManyPages;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _tournamentResults = new MtgMeleeClient().GetTournaments(new DateTime(2023, 09, 01), new DateTime(2023, 09, 07));
            _tournamentResultsManyPages = new MtgMeleeClient().GetTournaments(new DateTime(2023, 09, 01), new DateTime(2023, 09, 12));
        }

        [Test]
        public void ShouldHaveCorrectCount()
        {
            _tournamentResults.Length.Should().Be(23);
        }

        [Test]
        public void ShouldHaveCorrectResults()
        {
            _tournamentResults.First().Should().BeEquivalentTo(new MtgMeleeListTournamentInfo()
            {
                ID = 25360,
                Name = "Legacy League Pavia 23/24 - Tappa 12",
                Organizer = "Legacy Pavia",
                Date = new DateTime(2023, 09, 07, 19, 00, 00, DateTimeKind.Utc),
                Formats = new string[] { "Legacy" },
                Uri = new Uri("https://melee.gg/Tournament/View/25360"),
                Decklists = 13
            });
        }

        [Test]
        public void ShouldHaveCorrectCountForMultiPageRequest()
        {
            _tournamentResultsManyPages.Length.Should().Be(41);
        }
    }
}
