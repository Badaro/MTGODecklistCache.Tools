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
    }
}
