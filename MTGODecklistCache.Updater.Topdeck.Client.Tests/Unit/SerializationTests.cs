using FluentAssertions;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Tests.Unit
{
    public class SerializationTests
    {
        [Test]
        [TestCase(Game.MagicTheGathering, "Magic: The Gathering")]
        public void ShouldSerializeGameCorrectly(Game gameType, string expected)
        {
            dynamic? jsonObject = JsonConvert.DeserializeObject<dynamic>(new TopdeckTournamentRequest() { Game = gameType }.ToJson());
            string? game = jsonObject?.game;
            game.Should().Be(expected);
        }

        [Test]
        [TestCase(Format.EDH, "EDH")]
        [TestCase(Format.PauperEDH, "Pauper EDH")]
        [TestCase(Format.Standard, "Standard")]
        [TestCase(Format.Pioneer, "Pioneer")]
        [TestCase(Format.Modern, "Modern")]
        [TestCase(Format.Legacy, "Legacy")]
        [TestCase(Format.Pauper, "Pauper")]
        [TestCase(Format.Vintage, "Vintage")]
        [TestCase(Format.Premodern, "Premodern")]
        [TestCase(Format.Limited, "Limited")]
        [TestCase(Format.Timeless, "Timeless")]
        [TestCase(Format.Historic, "Historic")]
        [TestCase(Format.Explorer, "Explorer")]
        [TestCase(Format.Oathbreaker, "Oathbreaker")]
        public void ShouldSerializeFormatCorrectly(Format formatType, string expected)
        {
            dynamic? jsonObject = JsonConvert.DeserializeObject<dynamic>(new TopdeckTournamentRequest() { Format = formatType }.ToJson());
            string? format = jsonObject?.format;
            format.Should().Be(expected);
        }

        [Test]
        [TestCase(PlayerColumn.Name, "name")]
        [TestCase(PlayerColumn.Decklist, "decklist")]
        [TestCase(PlayerColumn.Wins, "wins")]
        [TestCase(PlayerColumn.WinsSwiss, "winsSwiss")]
        [TestCase(PlayerColumn.WinsBracket, "winsBracket")]
        [TestCase(PlayerColumn.WinRate, "winRate")]
        [TestCase(PlayerColumn.WinRateSwiss, "winRateSwiss")]
        [TestCase(PlayerColumn.WinRateBracket, "winRateBracket")]
        [TestCase(PlayerColumn.Draws, "draws")]
        [TestCase(PlayerColumn.Losses, "losses")]
        [TestCase(PlayerColumn.LossesSwiss, "lossesSwiss")]
        [TestCase(PlayerColumn.LossesBracket, "lossesBracket")]
        [TestCase(PlayerColumn.ID, "id")]
        public void ShouldSerializePlayerColumnCorrectly(PlayerColumn columnType, string expected)
        {
            dynamic? jsonObject = JsonConvert.DeserializeObject<dynamic>(new TopdeckTournamentRequest() { Columns = new PlayerColumn[] { columnType } }.ToJson());
            string? column = jsonObject?.columns[0];
            column.Should().Be(expected);
        }

        [Test]
        public void ShouldSerializeSampleTournamentRequestCorrectly()
        {
            TopdeckTournamentRequest request = new TopdeckTournamentRequest()
            {
                Game = Game.MagicTheGathering,
                Start = 10000,
                End = 20000,
                Columns = new PlayerColumn[] { PlayerColumn.Name, PlayerColumn.Wins }
            };

            TopdeckTournamentRequest? jsonObject = JsonConvert.DeserializeObject<TopdeckTournamentRequest>(request.ToJson());

            Game? game = jsonObject?.Game;
            long? start = jsonObject?.Start;
            long? end = jsonObject?.End;
            PlayerColumn[]? columns = jsonObject?.Columns;

            game.Should().Be(request.Game);
            start.Should().Be(request.Start);
            end.Should().Be(request.End);
            columns.Should().BeEquivalentTo(request.Columns);
        }
    }
}