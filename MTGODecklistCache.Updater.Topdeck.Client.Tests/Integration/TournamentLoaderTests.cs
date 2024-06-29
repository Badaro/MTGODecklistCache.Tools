using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class TournamentLoaderTests
    {
        TopdeckTournament _info;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _info = new TopdeckClient().GetTournament("SrJAEZ8vbglVge29fG7l");
        }

        [Test]
        public void TournamentInfoShouldHaveData()
        {
            _info.Data.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveRounds()
        {
            _info.Rounds.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveStandings()
        {
            _info.Standings.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveValidData()
        {
            _info.Data.Should().BeEquivalentTo(new TopdeckTournamentInfo()
            {
                Name = "CCS Summer Showdown Modern 2k",
                StartDate = 1717934400,
                Game = Game.MagicTheGathering,
                Format = Format.Modern
            }); ;
        }
    }
}
