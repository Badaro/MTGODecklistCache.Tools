using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace MTGODecklistCache.Updater.Topdeck.Client.Constants
{
    public class Routes
    {
        public static readonly string RootUrl = "https://topdeck.gg/api";
        public static readonly string TournamentRoute = $"{RootUrl}/v2/tournaments";
        public static readonly string StandingsRoute = $"{RootUrl}/v2/tournaments/{{TID}}/standings";
    }
}
