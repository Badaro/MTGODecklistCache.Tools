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
                Format = Format.Legacy,
                Start = new DateTimeOffset(2024, 03, 30, 00, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
                End = new DateTimeOffset(2024, 03, 31, 00, 00, 00, TimeSpan.Zero).ToUnixTimeSeconds(),
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
                ID = "HxSr6p4bZXYjUMTibl8i",
                Name = "Spellbound Games Legacy Dual Land Tournament",
                StartDate = 1711803600,
                Uri = new Uri("https://topdeck.gg/event/HxSr6p4bZXYjUMTibl8i"),
            }, c => c.Excluding(t => t.Standings));
            _tournaments.First().Standings.Count().Should().Be(36);
            _tournaments.First().Standings.First().Should().BeEquivalentTo(
                new TopdeckListTournamentStanding
                {
                    Wins = 7,
                    Losses = 1,
                    Draws = 1,
                    Name = "Ryan Waligóra"
               }
            );
            _tournaments.First().Standings.Skip(1).First().DeckSnapshot.Should().BeEquivalentTo(
                new TopdeckListTournamentDeckSnapshot()
                {
                    Mainboard = new Dictionary<string, int>()
                    {
                        ["Island"] = 1, ["Plains"] = 1, ["Forest"] = 1, ["Uro, Titan of Nature's Wrath"] = 3, ["Brainstorm"] = 4, ["Daze"] = 1, ["Swords to Plowshares"] = 4, ["Life from the Loam"] = 1, ["Savannah"] = 1, ["Tundra"] = 3, ["Tropical Island"] = 2, ["Flooded Strand"] = 4, ["Force of Will"] = 4, ["Wasteland"] = 2, ["Misty Rainforest"] = 4, ["Phyrexian Dreadnought"] = 4, ["Batterskull"] = 1, ["Stoneforge Mystic"] = 4, ["Stifle"] = 3, ["Ponder"] = 4, ["Prismatic Ending"] = 1, ["Dress Down"] = 3, ["Kaldra Compleat"] = 1, ["Lórien Revealed"] = 1, ["Hedge Maze"] = 1, ["Cryptic Coat"] = 2
                    },
                    Sideboard = new Dictionary<string, int>()
                    {
                        ["Deafening Silence"] = 1, ["Blue Elemental Blast"] = 1, ["Carpet of Flowers"] = 1, ["Containment Priest"] = 1, ["Veil of Summer"] = 1, ["Hydroblast"] = 1, ["Surgical Extraction"] = 2, ["Lavinia, Azorius Renegade"] = 1, ["Flusterstorm"] = 1, ["Force of Negation"] = 1, ["Torpor Orb"] = 1, ["Powder Keg"] = 1, ["Hullbreacher"] = 1, ["Boseiju, Who Endures"] = 1
                    }
                }
            );;
        }

        [Test]
        public void EmptyDecklistsShouldBeNull()
        {
            _tournaments.SelectMany(t => t.Standings).Where(s => s.DeckSnapshot != null).Should().AllSatisfy(d => d.DeckSnapshot.Mainboard.Should().NotBeNullOrEmpty());
        }
    }
}
