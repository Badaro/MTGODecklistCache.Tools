using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Integration
{
    public class TournamentInfoLoaderTests
    {
        TopdeckTournamentInfo _info;

        [OneTimeSetUp]
        public void LoadTournaments()
        {
            _info = new TopdeckClient().GetTournamentInfo("SrJAEZ8vbglVge29fG7l");
        }

        [Test]
        public void TournamentInfoShouldHaveName()
        {
            _info.Name.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void TournamentInfoShouldHaveGame()
        {
            _info.Game.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveFormat()
        {
            _info.Format.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveStartDate()
        {
            _info.StartDate.Should().NotBeNull();
        }

        [Test]
        public void TournamentInfoShouldHaveValidData()
        {
            _info.Should().BeEquivalentTo(new TopdeckTournamentInfo()
            {
                Name = "CCS Summer Showdown Modern 2k",
                StartDate = 1717934400,
                Game = Game.MagicTheGathering,
                Format = Format.Modern
            }); ;
        }

    }
}
