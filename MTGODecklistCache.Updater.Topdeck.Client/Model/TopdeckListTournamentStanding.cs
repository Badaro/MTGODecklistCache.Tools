using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckListTournamentStanding : NormalizableObject
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("wins")]
        public int? Wins { get; set; }
        [JsonProperty("losses")]
        public int? Losses { get; set; }
        [JsonProperty("draws")]
        public int? Draws { get; set; }
        [JsonProperty("deckSnapshot")]
        public TopdeckListTournamentDeckSnapshot? DeckSnapshot { get; set; }

        public void Normalize()
        {
        }
    }
}
