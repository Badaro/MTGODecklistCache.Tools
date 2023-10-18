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
                });
        }

        [Test]
        public void ShouldAddFormatToNameIfMissing()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/17469"))
                .First()
                .Should()
                .BeEquivalentTo(new MtgMeleeTournament()
                {
                    Name = "MXP Portland Oct 15 ReCQ (Modern)",
                    Date = new DateTime(2023, 10, 15, 21, 00, 00, DateTimeKind.Utc).ToUniversalTime(),
                    Uri = new Uri("https://melee.gg/Tournament/View/17469")
                });
        }

        [Test]
        public void ShouldNotIncludeOtherFormatNamesInName()
        {
            new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/12521"))
                .First().Name
                .Should().Contain("Pioneer")
                .And.NotContain("Legacy");
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
        public void ShouldDetectFormatForTeamTournamentsWithSingleFormatListed()
        {
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/16136"));

            result.Length.Should().Be(3);

            result[0].DeckOffset.Should().Be(0);
            result[0].Name.Should().Contain("Legacy");
            result[0].ExpectedDecks.Should().Be(3);
            result[0].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[1].DeckOffset.Should().Be(1);
            result[1].Name.Should().Contain("Modern");
            result[1].ExpectedDecks.Should().Be(3);
            result[1].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[2].DeckOffset.Should().Be(2);
            result[2].Name.Should().Contain("Pioneer");
            result[2].ExpectedDecks.Should().Be(3);
            result[2].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithMultipleFormatsListed()
        {
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(new Uri("https://melee.gg/Tournament/View/15645"));

            result.Length.Should().Be(3);

            result[0].DeckOffset.Should().Be(0);
            result[0].Name.Should().Contain("Legacy");
            result[0].ExpectedDecks.Should().Be(3);
            result[0].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[1].DeckOffset.Should().Be(1);
            result[1].Name.Should().Contain("Modern");
            result[1].ExpectedDecks.Should().Be(3);
            result[1].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);

            result[2].DeckOffset.Should().Be(2);
            result[2].Name.Should().Contain("Pioneer");
            result[2].ExpectedDecks.Should().Be(3);
            result[2].FixBehavior.Should().Be(MtgMeleeMissingDeckBehavior.Skip);
        }

    }
}