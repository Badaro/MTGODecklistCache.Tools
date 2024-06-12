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
        public void ShouldIgnoreTournamentsWithPhasesButNoDecklists()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 31590);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
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
        public void ShouldIgnoreSmallTournaments()
        {
            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 24659);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithSingleFormatListed()
        {
            Assert.Ignore();

            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 16136);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result.Length.Should().Be(3);

            result[0].DeckOffset.Should().Be(0);
            result[0].JsonFile.Should().Contain("legacy");
            result[0].ExpectedDecks.Should().Be(3);
            result[0].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[1].DeckOffset.Should().Be(1);
            result[1].JsonFile.Should().Contain("modern");
            result[1].ExpectedDecks.Should().Be(3);
            result[1].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[2].DeckOffset.Should().Be(2);
            result[2].JsonFile.Should().Contain("pioneer");
            result[2].ExpectedDecks.Should().Be(3);
            result[2].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithMultipleFormatsListed()
        {
            Assert.Ignore();

            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 07, 08, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 08, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 15645);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result.Length.Should().Be(3);

            result[0].DeckOffset.Should().Be(0);
            result[0].JsonFile.Should().Contain("legacy");
            result[0].ExpectedDecks.Should().Be(3);
            result[0].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[1].DeckOffset.Should().Be(1);
            result[1].JsonFile.Should().Contain("modern");
            result[1].ExpectedDecks.Should().Be(3);
            result[1].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[2].DeckOffset.Should().Be(2);
            result[2].JsonFile.Should().Contain("pioneer");
            result[2].ExpectedDecks.Should().Be(3);
            result[2].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithSameFormatInAllSeats()
        {
            Assert.Ignore();

            var tournament = new MtgMeleeClient().GetTournaments(
                new DateTime(2023, 09, 30, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 09, 30, 00, 00, 00, DateTimeKind.Utc))
                .First(t => t.ID == 17900);
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result.Length.Should().Be(3);

            result[0].DeckOffset.Should().Be(0);
            result[0].JsonFile.Should().Contain("pauper");
            result[0].ExpectedDecks.Should().Be(3);
            result[0].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[1].DeckOffset.Should().Be(1);
            result[1].JsonFile.Should().Contain("pauper");
            result[1].ExpectedDecks.Should().Be(3);
            result[1].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
            result[1].JsonFile.Should().NotBe(result[0].JsonFile);

            result[2].DeckOffset.Should().Be(2);
            result[2].JsonFile.Should().Contain("pauper");
            result[2].ExpectedDecks.Should().Be(3);
            result[2].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
            result[2].JsonFile.Should().NotBe(result[0].JsonFile);
            result[2].JsonFile.Should().NotBe(result[1].JsonFile);
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
            result.ExpectedDecks.Should().Be(3);
            result.DeckOffset.Should().Be(0);
            result.FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.UseFirst);
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("modern");
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
            result.ExpectedDecks.Should().BeNull();
            result.DeckOffset.Should().BeNull();
            result.FixBehavior.Should().Be(default(MtgMeleeMissingDeckBehavior));
            result.ExcludedRounds.Should().BeNull();
            result.JsonFile.Should().Contain("standard");
        }
    }
}