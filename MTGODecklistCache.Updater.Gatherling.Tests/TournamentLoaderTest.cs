using FluentAssertions;
using MTGODecklistCache.Updater.Model;

namespace MTGODecklistCache.Updater.Gatherling.Tests
{
    public class TournamentLoaderTest
    {
        CacheItem _tournament;

        [SetUp]
        public void Setup()
        {
            _tournament = new GatherlingSource().GetTournamentDetails(new Tournament()
            {
                Name = "The Penny Dreadful 500 (Season 35)",
                Uri = new Uri("https://gatherling.com/eventreport.php?event=The%20Penny%20Dreadful%20500%20(Season%2035)"),
                Date = new DateTime(2024, 11, 16, 13, 30, 00, DateTimeKind.Utc)
            });
        }

        [Test]
        public void ShouldLoadTournament()
        {
            _tournament.Should().NotBeNull();
        }

        [Test]
        public void ShouldLoadCorrectTournamentInfo()
        {
            _tournament.Tournament.Name.Should().Be("The Penny Dreadful 500 (Season 35)");
            _tournament.Tournament.Uri.Should().Be(new Uri("https://gatherling.com/eventreport.php?event=The%20Penny%20Dreadful%20500%20(Season%2035)"));
            _tournament.Tournament.Date.Should().Be(new DateTime(2024, 11, 16, 13, 30, 00, DateTimeKind.Utc));
        }

        [Test]
        public void ShouldGenerateJsonFile()
        {
            _tournament.Tournament.JsonFile.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void ShouldGenerateJsonFileWithFormatForPennyDreadful()
        {
            _tournament.Tournament.JsonFile.Should().Contain("penny-dreadful");
        }

        [Test]
        public void ShouldGenerateJsonFileWithFormatForPreModern()
        {
            CacheItem tournament = new GatherlingSource().GetTournamentDetails(new Tournament()
            {
                Name = "Pre-Modern Monthly League 14.06",
            });

            tournament.Tournament.JsonFile.Should().Contain("premod");
            tournament.Tournament.JsonFile.Should().NotContain("modern");
        }

        [Test]
        public void ShouldGenerateJsonFileWithFormatForPreFire()
        {
            CacheItem tournament = new GatherlingSource().GetTournamentDetails(new Tournament()
            {
                Name = "Pre-Fire Pop Ups 1.01",
            });

            tournament.Tournament.JsonFile.Should().Contain("prefire");
            tournament.Tournament.JsonFile.Should().NotContain("modern");
        }

        [Test]
        public void ShouldAddMissingFormatToJsonFile()
        {
            CacheItem tournament = new GatherlingSource().GetTournamentDetails(new Tournament()
            {
                Name = "2024 CPS Showdown November #3",
            });

            tournament.Tournament.JsonFile.Should().Contain("premod");
            tournament.Tournament.JsonFile.Should().NotContain("modern");
        }

        [Test]
        public void ShouldNotBreakOnInvalidTournament()
        {
            CacheItem tournament = new GatherlingSource().GetTournamentDetails(new Tournament()
            {
                Name = "DOES NOT EXIST",
            });

            tournament.Should().BeNull();
        }

        [Test]
        public void ShouldLoadStandings()
        {
            _tournament.Standings.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void ShouldLoadCorrectStandingsInfo()
        {
            _tournament.Standings.Skip(1).First().Should().BeEquivalentTo(new Standing()
            {
                Player = "Tygrak",
                Rank = 2,
                Points = 15,
                Wins = 5,
                Losses = 1,
                Draws = 0,
                GWP = 0.667,
                OGWP = 0.655,
                OMWP = 0.707
            });
        }
    }
}