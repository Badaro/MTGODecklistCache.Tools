using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class StandingLoaderTests
    {
        TopdeckStanding[]? _standings;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _standings = new TopdeckClient().GetStandings("iCMd298218qbEqeGt5d7");
        }

        [Test]
        public void StandingsShouldHavePlayerName()
        {
            _standings.Should().AllSatisfy(s => s.PlayerName.Should().NotBeNullOrEmpty());
        }
    }
}
