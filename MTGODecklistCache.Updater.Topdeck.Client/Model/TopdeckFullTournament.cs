using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Model
{
    public class TopdeckFullTournament : NormalizableObject
    {
        [JsonProperty("data")]
        public TopdeckTournamentInfo? Data { get; set; }

        [JsonProperty("standings")]
        public TopdeckStanding[]? Standings { get; set; }

        [JsonProperty("rounds")]
        public TopdeckRound[]? Rounds { get; set; }

        public void Normalize()
        {
        }
    }
}
