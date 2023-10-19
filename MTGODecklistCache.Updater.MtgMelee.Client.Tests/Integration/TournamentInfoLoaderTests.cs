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
        MtgMeleeTournamentInfo _tournament;
        MtgMeleeTournamentInfo _tournamentWithMultipleFormats;
        MtgMeleeTournamentInfo _tournamentWithoutDecklists;

        [OneTimeSetUp]
        public void LoadTournament()
        {
            _tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/31121"));
            _tournamentWithMultipleFormats = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/11717"));
            _tournamentWithoutDecklists = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/31576"));
        }

        [Test]
        public void ShouldLoadTournamentData()
        {
            _tournament.Should().BeEquivalentTo(new MtgMeleeTournamentInfo()
            {
                ID = 31121,
                Organizer = "PharaohTorneios",
                Name = "Pharaoh's Shop - Legacy RS 2023 - Super Legacy",
                Date = new DateTime(2023, 10, 01, 13, 00, 00, DateTimeKind.Utc),
                Formats = new string[] { "Legacy" },
                Uri = new Uri("https://melee.gg/Tournament/View/31121")
            });
        }


        [Test]
        public void ShouldLoadTournamentDateAsUtc()
        {
            _tournament.Date.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Test]
        public void ShouldLoadTournamentFormatInMultiFormatTournament()
        {
            _tournamentWithMultipleFormats.Formats.Should().BeEquivalentTo(new string[] { "Pioneer", "Modern", "Legacy" });
        }
    }
}
