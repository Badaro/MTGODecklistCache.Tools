using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckTournamentRequest
    {
        [JsonProperty("game", NullValueHandling = NullValueHandling.Ignore)]
        public Game Game { get; set; }
        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public Format? Format { get; set; }
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public long? Start { get; set; }
        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public long? End { get; set; }
        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public int? Last { get; set; }
        [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
        public PlayerColumn[]? Columns { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
