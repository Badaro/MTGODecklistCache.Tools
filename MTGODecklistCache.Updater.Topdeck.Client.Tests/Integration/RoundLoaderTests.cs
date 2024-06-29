using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class RoundLoaderTests
    {
        TopdeckRound[]? _rounds;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _rounds = new TopdeckClient().GetRounds("iCMd298218qbEqeGt5d7");
        }

        [Test]
        public void RoundsShouldHaveNumbers()
        {
            _rounds.Should().AllSatisfy(r => r.Name.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void RoundsShouldHaveTables()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().NotBeNull());
        }

        [Test]
        public void RoundTablesShouldHaveNumbers()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().AllSatisfy(t => t.Number.Should().BeGreaterThan(0)));
        }

        [Test]
        public void RoundTablesShouldHavePlayers()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().AllSatisfy(t => t.Players.Should().NotBeNull()));
        }

        [Test]
        public void RoundTablesShouldHaveWinners()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().AllSatisfy(t => t.Winner.Should().NotBeNullOrEmpty()));
        }

        [Test]
        public void RoundTablesShouldHaveWinnersMatchingOneOfThePlayersOrDraw()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().AllSatisfy(t => t.Winner.Should().BeOneOf(t.Players.Select(p => p.Name).Append(Misc.DrawText))));
        }

        [Test]
        public void RoundTablesShouldHavePlayerNames()
        {
            _rounds.Should().AllSatisfy(r => r.Tables.Should().AllSatisfy(t => t.Players.Should().AllSatisfy(p => p.Name.Should().NotBeNullOrEmpty())));
        }
    }
}
