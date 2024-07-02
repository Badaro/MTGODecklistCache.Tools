using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckListTournamentDeckSnapshot : NormalizableObject
    {
        [JsonProperty("mainboard")]
        public Dictionary<string, int>? Mainboard { get; set; }
        [JsonProperty("sideboard")]
        public Dictionary<string,int>? Sideboard { get; set; }

        public void Normalize()
        {
        }
    }
}
