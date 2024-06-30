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
                Format = Format.Modern,
                Start = new DateTimeOffset(2024, 04, 01, 00, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                End = new DateTimeOffset(2024, 04, 30, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                Columns = new[] { PlayerColumn.Name, PlayerColumn.ID, PlayerColumn.Decklist, PlayerColumn.Wins, PlayerColumn.Losses, PlayerColumn.Draws }
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
                ID = "iCMd298218qbEqeGt5d7",
                Name = "Emjati - Eternal - Modern",
                StartDate = 1712296800,
                Uri = new Uri("https://topdeck.gg/event/iCMd298218qbEqeGt5d7"),
                Standings = new TopdeckListTournamentStanding[]
                {
                    new TopdeckListTournamentStanding { Wins=5, Losses=0, Draws=1, Name="Carlos Jiménez Moreno " },
                    new TopdeckListTournamentStanding { Wins=4, Losses=2, Draws=0, Name="Eric Bauwens" },
                    new TopdeckListTournamentStanding { Wins=3, Losses=2, Draws=0, Name="Raphael Lecocq" },
                    new TopdeckListTournamentStanding { Wins=2, Losses=1, Draws=2, Name="Gert\tCorthout" },
                    new TopdeckListTournamentStanding { Wins=2, Losses=1, Draws=1, Name="Fabian Ulloa Varela" },
                    new TopdeckListTournamentStanding { Wins=2, Losses=2, Draws=0, Name="Kevin Van Der Werf" },
                    new TopdeckListTournamentStanding { Wins=2, Losses=2, Draws=0, Name="Bart\tBoudewijn" },
                    new TopdeckListTournamentStanding { Wins=1, Losses=2, Draws=1, Name="Luk Schoonaerr" },
                    new TopdeckListTournamentStanding { Wins=1, Losses=2, Draws=1, Name="Mathi Lenearts" },
                    new TopdeckListTournamentStanding { Wins=1, Losses=3, Draws=0, Name="Nicolas Di Gregorio" },
                    new TopdeckListTournamentStanding { Wins=1, Losses=3, Draws=0, Name="Jens Engelsma" },
                    new TopdeckListTournamentStanding { Wins=0, Losses=4, Draws=0, Name="Randolf De Couvreur" },
                }
            });
        }
    }
}
