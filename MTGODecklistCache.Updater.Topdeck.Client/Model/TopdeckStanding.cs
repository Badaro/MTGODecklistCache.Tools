using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckStanding : NormalizableObject
    {
        [JsonProperty("standing")]
        public int? Standing { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
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

        public void Normalize()
        {
            if (String.IsNullOrEmpty(this.Decklist) || this.Decklist == Misc.NoDecklistsText || !Uri.IsWellFormedUriString(this.Decklist, UriKind.Absolute)) this.Decklist = null;
        }
    }
}
