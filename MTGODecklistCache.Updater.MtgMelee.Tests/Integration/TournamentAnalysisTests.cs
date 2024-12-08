using NUnit.Framework;
using FluentAssertions;
using MTGODecklistCache.Updater.MtgMelee.Client;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;

namespace MTGODecklistCache.Updater.MtgMelee.Tests
{
    public class TournamentAnalysisTests
    {
        [Test]
        public void ShouldDetectSimpleTournaments()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 10, 14, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 14, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("17461"));

            result.Should()
                .BeEquivalentTo(new Tournament()
                {
                    Name = "MXP Portland Oct 14 Legacy 5k",
                    Date = new DateTime(2023, 10, 14, 19, 00, 00, DateTimeKind.Utc).ToUniversalTime(),
                    Uri = new Uri("https://melee.gg/Tournament/View/17461")
                }, o => o.Excluding(t => t.JsonFile));
        }

        [Test]
        public void ShouldAddFormatToJsonFileIfMissing()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("17469"));

            result.Should()
                .BeEquivalentTo(new Tournament()
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
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2022, 11, 19, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2022, 11, 19, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("12521"));

            result
                .JsonFile
                .Should().Contain("pioneer")
                .And.NotContain("legacy");
        }

        [Test]
        public void ShouldIgnoreTournamentsMissingMostDecklists()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("15653"));

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTournamentsWithInvalidFormats()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 10, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 10, 15, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("24658"));

            result
                .Should()
                .BeNull();
        }

        [Test]
        public void ShouldIgnoreTeamTournaments()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 15, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("16136"));

            result.Should().BeNull();
        }

        [Test]
        public void ShouldHandleProToursCorrectly()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 07, 28, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 28, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("16429"));

            result.Name.Should().Be("Pro Tour The Lord of the Rings");
            result.Date.Should().Be(new DateTime(2023, 07, 28, 07, 00, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/16429"));
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("modern");
        }

        [Test]
        public void ShouldHandleWorldsCorrectly()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2024, 10, 23, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2024, 10, 26, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("146430"));

            result.Name.Should().Be("World Championship 30 in Las Vegas");
            result.Date.Should().Be(new DateTime(2024, 10, 24, 16, 00, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/146430"));
            result.ExcludedRounds.Should().BeEquivalentTo(new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" });
            result.JsonFile.Should().Contain("standard");
        }

        [Test]
        public void ShouldNotConsiderWizardsQualifiersAsProTour()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2024, 04, 27, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2024, 04, 27, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("87465"));

            result.Name.Should().Be("SATURDAY 2nd Chance Pro Tour Qualifier");
            result.Date.Should().Be(new DateTime(2024, 04, 27, 16, 30, 00, DateTimeKind.Utc).ToUniversalTime());
            result.Uri.Should().Be(new Uri("https://melee.gg/Tournament/View/87465"));
            result.ExcludedRounds.Should().BeNull();
            result.JsonFile.Should().Contain("standard");
        }

        [Test]
        public void ShouldSkipTeamTournaments()
        {
            var result = new MtgMeleeSource().GetTournaments(
                new DateTime(2023, 07, 08, 00, 00, 00, DateTimeKind.Utc),
                new DateTime(2023, 07, 08, 23, 59, 59, DateTimeKind.Utc))
                .FirstOrDefault(t => t.Uri.ToString().Contains("15645"));

            result
                .Should()
                .BeNull();
        }
    }
}