using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class TournamentListLoaderTests
    {
        TopdeckListTournament[]? _tournaments;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _tournaments = new TopdeckClient().GetTournamentList(new TopdeckTournamentRequest()
            {
                Game = Game.MagicTheGathering,
                Format = Format.Pauper,
                Start = new DateTimeOffset(2024, 06, 01, 00, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                End = new DateTimeOffset(2024, 06, 30, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                Columns = new[] { PlayerColumn.Name, PlayerColumn.ID, PlayerColumn.Decklist, PlayerColumn.Wins, PlayerColumn.Losses, PlayerColumn.Draws, PlayerColumn.DeckSnapshot }
            });
        }

        [Test]
        public void ShouldLoadTournaments()
        {
            _tournaments.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void ShouldLoadExpectedNumberOfTournaments()
        {
            _tournaments.Should().HaveCount(2);
        }

        [Test]
        public void ShouldLoadTournamentIDs()
        {
            _tournaments.Should().AllSatisfy(t => t.ID.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldLoadTournamentNames()
        {
            _tournaments.Should().AllSatisfy(t => t.ID.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldLoadTournamentDates()
        {
            _tournaments.Should().AllSatisfy(t => t.StartDate.Should().BeGreaterThan(0));
        }

        [Test]
        public void ShouldLoadTournamentStandings()
        {
            _tournaments.Should().AllSatisfy(t => t.Standings.Should().NotBeNull());
        }

        [Test]
        public void ShouldLoadTournamentStandingsWins()
        {
            _tournaments.Should().AllSatisfy(t => t.Standings.Where(t => t.Wins > 0).Count().Should().BeGreaterThan(0));
        }

        [Test]
        public void ShouldLoadTournamentStandingsLosses()
        {
            _tournaments.Should().AllSatisfy(t => t.Standings.Where(t => t.Losses > 0).Count().Should().BeGreaterThan(0));
        }

        [Test]
        public void ShouldLoadTournamentStandingsDraws()
        {
            _tournaments.Should().AllSatisfy(t => t.Standings.Where(t => t.Draws > 0).Count().Should().BeGreaterThan(0));
        }

        [Test]
        public void ShouldLoadTournamentDeckSnapshot()
        {
            _tournaments.Should().AllSatisfy(t => t.Standings.Where(t => t.DeckSnapshot != null && t.DeckSnapshot.Mainboard != null && t.DeckSnapshot.Mainboard.Count > 0).Count().Should().BeGreaterThan(0));
        }

        [Test]
        public void ShouldLoadTournamentUrls()
        {
            _tournaments.Should().AllSatisfy(t => t.Uri.Should().NotBeNull());
            _tournaments.Should().AllSatisfy(t => t.Uri.ToString().Contains("topdeck.gg"));
        }

        [Test]
        public void ShouldLoadExpectedData()
        {
            _tournaments.First().Should().BeEquivalentTo(new TopdeckListTournament()
            {
                ID = "qQUHhYPx257AkYK2sqBB",
                Name = "MTGO Creator Showdown - Pauper",
                StartDate = 1718344800,
                Uri = new Uri("https://topdeck.gg/event/qQUHhYPx257AkYK2sqBB"),
            }, c => c.Excluding(t => t.Standings));
            _tournaments.First().Standings.Count().Should().Be(48);
            _tournaments.First().Standings.First().Should().BeEquivalentTo(
                new TopdeckListTournamentStanding
                {
                    Wins = 6,
                    Losses = 0,
                    Draws = 0,
                    Name = "El_Gran_Boa",
                    DeckSnapshot = new TopdeckListTournamentDeckSnapshot()
                    {
                        Mainboard = new Dictionary<string, int>() { ["Chromatic Star"] = 3, ["Frogmite"] = 2, ["Galvanic Blast"] = 4, ["Great Furnace"] = 3, ["Metallic Rebuke"] = 1, ["Thoughtcast"] = 4, ["Ichor Wellspring"] = 4, ["Myr Enforcer"] = 4, ["Seat of the Synod"] = 4, ["Vault of Whispers"] = 4, ["Makeshift Munitions"] = 2, ["Krark-Clan Shaman"] = 1, ["Drossforge Bridge"] = 3, ["Mistvault Bridge"] = 4, ["Silverbluff Bridge"] = 2, ["Deadly Dispute"] = 4, ["Blood Fountain"] = 3, ["Reckoner's Bargain"] = 4, ["Refurbished Familiar"] = 4 },
                        Sideboard = new Dictionary<string, int>() { ["Negate"] = 1, ["Hydroblast"] = 4, ["Pyroblast"] = 4, ["Gorilla Shaman"] = 2, ["Krark-Clan Shaman"] = 1, ["Cast into the Fire"] = 3 }
                    }
                }
            );
        }
    }
}
