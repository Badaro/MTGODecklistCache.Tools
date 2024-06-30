using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckTournamentInfo : NormalizableObject
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("game")]
        public Game? Game { get; set; }

        [JsonProperty("format")]
        public Format? Format { get; set; }

        [JsonProperty("startDate")]
        public long? StartDate { get; set; }

        public void Normalize()
        {
        }
    }
}
