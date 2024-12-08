using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using System.Collections.Generic;
using System.IO;
using MTGODecklistCache.Updater.MtgMelee.Client;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class ErrorHandlingTests
    {
        [Test]
        public void ShouldNotBreakOnEmptyTournaments()
        {
            var tournament = new MtgMeleeSource().GetTournamentDetails(new MtgMeleeTournament()
            {
                Uri = new Uri("https://melee.gg/Tournament/View/31590"),
                Date = new DateTime(2023, 10, 15, 09, 00, 00, DateTimeKind.Utc)
            });

            tournament.Should().NotBeNull();
            tournament.Decks.Should().NotBeNull().And.HaveCount(0);

        }
    }
}