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
            _standings = new TopdeckClient().GetStandings("SrJAEZ8vbglVge29fG7l");
        }

        [Test]
        public void StandingsShouldHavePlayerName()
        {
            _standings.Should().AllSatisfy(s => s.PlayerName.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void StandingsShouldHavePlayerID()
        {
            _standings.Where(s => !String.IsNullOrEmpty(s.PlayerID)).Should().NotBeNullOrEmpty();
        }

        [Test]
        public void StandingsShouldHaveDecklists()
        {
            _standings.Where(s => !String.IsNullOrEmpty(s.Decklist)).Should().NotBeNullOrEmpty();
        }

        [Test]
        public void StandingsShouldHavePoints()
        {
            _standings.Should().AllSatisfy(s => s.Points.Should().NotBeNull());
            _standings.Where(s => s.Points > 0).Should().NotBeEmpty();
        }

        [Test]
        public void StandingsShouldHaveStanding()
        {
            _standings.Should().AllSatisfy(s => s.Standing.Should().NotBeNull());
            _standings.Where(s => s.Standing > 0).Should().NotBeEmpty();
        }

        [Test]
        public void StandingsShouldHaveOpponentWinRate()
        {
            _standings.Should().AllSatisfy(s => s.OpponentWinRate.Should().NotBeNull());
            _standings.Where(s => s.OpponentWinRate > 0).Should().NotBeEmpty();
        }

        [Test]
        public void StandingsShouldHaveOpponentGameWinRate()
        {
            _standings.Should().AllSatisfy(s => s.OpponentGameWinRate.Should().NotBeNull());
            _standings.Where(s => s.OpponentGameWinRate > 0).Should().NotBeEmpty();
        }

        [Test]
        public void StandingsShouldHaveGameWinRate()
        {
            _standings.Should().AllSatisfy(s => s.GameWinRate.Should().NotBeNull());
            _standings.Where(s => s.GameWinRate > 0).Should().NotBeEmpty();
        }

        [Test]
        public void StandingsShouldHaveValidData()
        {
            _standings.First().Should().BeEquivalentTo(new TopdeckStanding()
            {
                Standing = 1,
                Points = 11,
                GameWinRate = 0.7407407407407407,
                OpponentGameWinRate = 0.68728956228956228,
                OpponentWinRate = 0.6333333333333333,
                PlayerName = "Kerry leamon",
                PlayerID = "akcSttW2MLWajg6LLvN2LiwOmnw2",
                Decklist = null
            });
        }
    }
}
