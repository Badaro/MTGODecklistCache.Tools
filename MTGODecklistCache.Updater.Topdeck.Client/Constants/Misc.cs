using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Constants
{
    public static class Misc
    {
        public static readonly string NoDecklistsText = "No Decklist Available";
        public static readonly string DrawText = "Draw";
        public static readonly string TournamentPage = "https://topdeck.gg/event/{tournamentId}";
    }
}
