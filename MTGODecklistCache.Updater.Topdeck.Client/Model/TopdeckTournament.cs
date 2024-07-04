using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckTournament : NormalizableObject
    {
        [JsonProperty("data")]
        public TopdeckTournamentInfo? Data { get; set; }

        [JsonProperty("standings")]
        public TopdeckStanding[]? Standings { get; set; }

        [JsonProperty("rounds")]
        public TopdeckRound[]? Rounds { get; set; }

        public void Normalize()
        {
            this.Data?.Normalize();
            this.Standings?.ToList().ForEach(s => s.Normalize());
            this.Rounds?.ToList().ForEach(r => r.Normalize());
        }
    }
}
