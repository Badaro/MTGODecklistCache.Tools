using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckListTournamentDeckSnapshot : NormalizableObject
    {
        [JsonProperty("mainboard")]
        public Dictionary<string, int>? Mainboard { get; set; }
        [JsonProperty("sideboard")]
        public Dictionary<string, int>? Sideboard { get; set; }

        public void Normalize()
        {
            if (this.Mainboard != null && this.Mainboard.Count() == 0) this.Mainboard = null;
            if (this.Sideboard != null && this.Sideboard.Count() == 0) this.Sideboard = null;
        }
    }
}
