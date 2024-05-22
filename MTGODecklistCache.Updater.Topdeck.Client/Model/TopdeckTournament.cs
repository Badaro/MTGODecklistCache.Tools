using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckTournament
    {
        [JsonProperty("TID")]
        public string? TID { get; set; }
        [JsonProperty("tournamentName")]
        public string? Name { get; set; }
        [JsonProperty("startDate")]
        public long? StartDate { get; set; }
    }
}
