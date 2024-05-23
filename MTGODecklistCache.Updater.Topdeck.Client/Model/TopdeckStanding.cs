using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckStanding
    {
        [JsonProperty("standing")]
        public int? Standing { get; set; }
        [JsonProperty("name")]
        public string? PlayerName { get; set; }
        [JsonProperty("discord")]
        public string? Discord { get; set; }
        [JsonProperty("id")]
        public string? PlayerID { get; set; }
        [JsonProperty("decklist")]
        public string? Decklist { get; set; }
        [JsonProperty("points")]
        public int? Points { get; set; }
        [JsonProperty("opponentWinRate")]
        public double? OpponentWinRate { get; set; }
        [JsonProperty("gameWinRate")]
        public double? GameWinRate { get; set; }
        [JsonProperty("opponentGameWinRate")]
        public double? OpponentGameWinRate { get; set; }
    }
}
