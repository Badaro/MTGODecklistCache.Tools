using NUnit.Framework;
using FluentAssertions;
using MTGODecklistCache.Updater.MtgMelee.Client;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer.Tests
{
    public class Tests
    {
        [Test]
        public void ShouldDetectSimpleTournaments()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 14, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 14, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 17461);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result.Should()
                .BeEquivalentTo(new MtgMeleeTournament()
                {
                    Name = "MXP Portland Oct 14 Legacy 5k",
                    Date = new DateTime(2023, 10, 14, 19, 00, 00, DateTimeKind.Utc).ToUniversalTime(),
                    Uri = new Uri("https://melee.gg/Tournament/View/17461")
                }, o => o.Excluding(t => t.JsonFile));
        }

        [Test]
        public void ShouldAddFormatToJsonFileIfMissing()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 17469);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result.Should()
                .BeEquivalentTo(new MtgMeleeTournament()
                {
                    Name = "MXP Portland Oct 15 ReCQ",
                    Date = new DateTime(2023, 10, 15, 21, 00, 00, DateTimeKind.Utc).ToUniversalTime(),
                    Uri = new Uri("https://melee.gg/Tournament/View/17469")
                }, o => o.Excluding(t => t.JsonFile));
            result.JsonFile.Should().Contain("modern");
        }

        [Test]
        public void ShouldNotIncludeOtherFormatNamesInJsonFile()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2022, 11, 19, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2022, 11, 19, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 12521);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result
                .JsonFile
                .Should().Contain("pioneer")
                .And.NotContain("legacy");
        }

        [Test]
        public void ShouldIgnoreTournamentsMissingMostDecklists()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 15653);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsWithInvalidFormats()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 24658);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTeamTournaments()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 16136);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result.Should().BeNull();
        }

        [Test]
        public void ShouldHandleProToursCorrectly()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 07, 28, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 28, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 16429);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result.Name.Should().Be("Pro Tour The Lord of the Rings");
            result.Date.Should().Be(new DateTime(2023, 07, 28, 07, 00, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/16429"));
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("modern");
        }

        [Test]
        public void ShouldHandleWorldsCorrectly()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2024, 10, 23, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2024, 10, 26, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 146430);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result.Name.Should().Be("World Championship 30 in Las Vegas");
            result.Date.Should().Be(new DateTime(2024, 10, 24, 16, 00, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/146430"));
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("standard");
        }

        [Test]
        public void ShouldNotConsiderWizardsQualifiersAsProTour()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2024, 04, 27, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2024, 04, 27, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 87465);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result.Name.Should().Be("SATURDAY 2nd Chance Pro Tour Qualifier");
            result.Date.Should().Be(new DateTime(2024, 04, 27, 16, 30, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/87465"));
            result.ExcludedRounds.Should().BeNull();
            result.JsonFile.Should().Contain("standard");
        }

        [Test]
        public void ShouldSkipTeamTournamentsForNow()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 07, 08, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 08, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 15645);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }
    }
}