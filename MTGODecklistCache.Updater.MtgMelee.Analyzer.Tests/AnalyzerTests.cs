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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/17461"));
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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/17469"));
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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/12521"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

            result
                .JsonFile
                .Should().Contain("pioneer")
                .And.NotContain("legacy");
        }

        [Test]
        public void ShouldIgnoreTournamentsWithPhasesButNoDecklists()
        {
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/31590"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsMissingMostDecklists()
        {
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/15653"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsWithInvalidFormats()
        {
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/24658"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreSmallTournaments()
        {
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/27636"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldDetectFormatForTeamTournamentsWithSingleFormatListed()
        {
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/16136"));
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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/15645"));
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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/17900"));
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
            var tournament = new MtgMeleeClient().GetTournament(new Uri("https://melee.gg/Tournament/View/16429"));
            var result = new MtgMeleeAnalyzer().GetScraperTournaments(tournament).First();

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