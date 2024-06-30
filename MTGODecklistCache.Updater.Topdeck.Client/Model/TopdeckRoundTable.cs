using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckRoundTable : NormalizableObject
    {
        [JsonProperty("table")]
        public string? Name { get; set; }

        [JsonProperty("players")]
        public TopdeckRoundTablePlayer[]? Players { get; set; }

        [JsonProperty("winner")]
        public string? Winner { get; set; }

        public void Normalize()
        {
        }
    }
}
