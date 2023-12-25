using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class PageErrorTests
    {

        [Test]
        public void ShouldNotBreakOnUnavailableTournaments()
        {
            this.Invoking(x =>
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/2018-mocs-monthly-limited---1-entry-per-account-2018-10-2011664420")
            })).Should().NotThrow();
        }

        [Test]
        public void ShouldNotBreakOnEmptyLeagues()
        {
            this.Invoking(x =>
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/standard-league-2023-12-052852")
            })).Should().NotThrow();
        }
    }
}