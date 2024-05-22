using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class TournamentLoaderTests
    {
        TopdeckTournament[]? _tournaments;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _tournaments = new TopdeckClient().GetTournaments(new TopdeckTournamentRequest()
            {
                Game = Game.MagicTheGathering,
                Format = Format.Modern,
                Start = new DateTimeOffset(2024, 04, 01, 00, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                End = new DateTimeOffset(2024, 04, 30, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                Columns = new[] {PlayerColumn.Name, PlayerColumn.ID, PlayerColumn.Decklist }
            });
        }

        [Test]
        public void ShouldLoadTournaments()
        {
            _tournaments.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void ShouldLoadExpectedNumberTournaments()
        {
            _tournaments.Should().HaveCount(2);
        }

        [Test]
        public void ShouldLoadTournamentIDs()
        {
            _tournaments.Should().AllSatisfy(t => t.TID.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldLoadTournamentNames()
        {
            _tournaments.Should().AllSatisfy(t => t.TID.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldLoadTournamentDates()
        {
            _tournaments.Should().AllSatisfy(t => t.StartDate.Should().BeGreaterThan(0));
        }
    }
}
