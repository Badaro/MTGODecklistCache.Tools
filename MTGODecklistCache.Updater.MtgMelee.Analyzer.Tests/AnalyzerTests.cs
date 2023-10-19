using NUnit.Framework;
using FluentAssertions;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer.Tests
{
    public class Tests
    {
        [Test]
        public void ShouldDetectSimpleTournaments()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/17461"))
                .First()
                .Should()
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
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/17469")).First();

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
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/12521"))
                .First().JsonFile
                .Should().Contain("pioneer")
                .And.NotContain("legacy");
        }

        [Test]
        public void ShouldIgnoreTournamentsWithPhasesButNoDecklists()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/31590"))
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsMissingMostDecklists()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/15653"))
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsWithInvalidFormats()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/24658"))
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreSmallTournaments()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/27636"))
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithSingleFormatListed()
        {
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/16136"));

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
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/15645"));

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
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/17900"));

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
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/16429")).First();

            result.Name.Should().Be("Pro Tour The Lord of the Rings");
            result.Date.Should().Be(new DateTime(2023, 07, 28, 07, 00, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/16429"));
            result.ExpectedDecks.Should().Be(3);
            result.DeckOffset.Should().Be(2);
            result.FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.UseLast);
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("modern");
        }
    }
}