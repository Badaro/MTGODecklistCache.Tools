using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckListTournament : NormalizableObject
    {
        [JsonProperty("TID")]
        public string? ID { get; set; }
        [JsonProperty("tournamentName")]
        public string? Name { get; set; }
        [JsonProperty("startDate")]
        public long? StartDate { get; set; }
        public Uri? Uri { get; set; }
        [JsonProperty("standings")]
        public TopdeckListTournamentStanding[] Standings { get;set; }

        public void Normalize()
        {
            if (this.ID != null) this.Uri = new Uri(Misc.TournamentPage.Replace("{tournamentId}", this.ID));
        }
    }
}
