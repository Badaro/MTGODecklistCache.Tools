using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckRoundTablePlayer : NormalizableObject
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        public void Normalize()
        {
        }
    }
}
