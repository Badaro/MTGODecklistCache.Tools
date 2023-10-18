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
    internal class TournamentInfoLoaderTests
    {
        MtgMeleeTournament _tournament;
        MtgMeleeTournament _tournamentWithMultipleFormats;
        MtgMeleeTournament _tournamentWithoutDecklists;

        [SetUp]
        public void LoadTournament()
        {
            _tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/31121"));
            _tournamentWithMultipleFormats = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/11717"));
            _tournamentWithoutDecklists = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/31576"));
        }

        [Test]
        public void ShouldLoadTournamentID()
        {
            _tournament.ID.Should().Be(31121);
        }

        [Test]
        public void ShouldLoadTournamentName()
        {
            _tournament.Name.Should().Be("Pharaoh's Shop - Legacy RS 2023 - Super Legacy");
        }

        [Test]
        public void ShouldLoadTournamentDate()
        {
            _tournament.Date.Should().Be(new DateTime(2023, 10, 01, 13, 00, 00, DateTimeKind.Utc));
        }

        [Test]
        public void ShouldLoadTournamentDateAsUtc()
        {
            _tournament.Date.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Test]
        public void ShouldLoadTournamentFormat()
        {
            _tournament.Formats.Should().BeEquivalentTo(new string[] { "Legacy" });
        }

        [Test]
        public void ShouldLoadTournamentFormatInMultiFormatTournament()
        {
            _tournamentWithMultipleFormats.Formats.Should().BeEquivalentTo(new string[] { "Pioneer", "Modern", "Legacy" });
        }

        [Test]
        public void ShouldDetectTournamentsWithDecklists()
        {
            _tournament.MaxDecklists.Should().BePositive();
        }

        [Test]
        public void ShouldDetectTournamentsWithoutDecklists()
        {
            _tournamentWithoutDecklists.MaxDecklists.Should().Be(0);
        }

        [Test]
        public void ShouldDetectMaxCountForNormalTournaments()
        {
            _tournament.MaxDecklists.Should().Be(1);
        }

        [Test]
        public void ShouldDetectMaxCountForTeamTournaments()
        {
            _tournamentWithMultipleFormats.MaxDecklists.Should().Be(3);
        }
    }
}
