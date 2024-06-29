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

        [Test]
        public void StandingsShouldHavePoints()
        {
            _standings.Should().AllSatisfy(s => s.Points.Should().NotBeNull());
        }

        [Test]
        public void StandingsShouldHaveStanding()
        {
            _standings.Should().AllSatisfy(s => s.Standing.Should().NotBeNull());
        }

        [Test]
        public void StandingsShouldHaveOpponentWinRate()
        {
            _standings.Should().AllSatisfy(s => s.OpponentWinRate.Should().NotBeNull());
        }

        [Test]
        public void StandingsShouldHaveOpponentGameWinRate()
        {
            _standings.Should().AllSatisfy(s => s.OpponentGameWinRate.Should().NotBeNull());
        }

        [Test]
        public void StandingsShouldHaveGameWinRate()
        {
            _standings.Should().AllSatisfy(s => s.GameWinRate.Should().NotBeNull());
        }

        [Test]
        public void StandingsShouldHaveValidData()
        {
            _standings.First().Should().BeEquivalentTo(new TopdeckStanding()
            {
                Standing = 1,
                Points = 10,
                GameWinRate = 0.73333333333333328,
                OpponentGameWinRate = 0.57777777777777772,
                OpponentWinRate = 0.58333333333333337,
                PlayerName = "Carlos Jiménez Moreno ",
                Decklist = null
            });
        }
    }
}
