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
    }
}