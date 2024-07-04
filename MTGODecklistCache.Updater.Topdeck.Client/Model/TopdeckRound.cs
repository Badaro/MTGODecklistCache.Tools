using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckRound : NormalizableObject
    {
        [JsonProperty("round")]
        public string? Name { get; set; }

        [JsonProperty("tables")]
        public TopdeckRoundTable[]? Tables { get; set; }

        public void Normalize()
        {
            this.Tables?.ToList().ForEach(t => t.Normalize());
        }
    }
}
