using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using MTGODecklistCache.Updater.Topdeck.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.Topdeck.Client
{
    public class MissingApiKeyException : Exception
    {
        public MissingApiKeyException() : base($"Could not load API key from environment variable {Settings.ApiKeyEnvVar}")
        {
        }
    }

    public class TopdeckClient
    {
        private string? _apiKey;

        public TopdeckClient()
        {
            _apiKey = Environment.GetEnvironmentVariable(Settings.ApiKeyEnvVar)?.Trim();
            if (String.IsNullOrEmpty(_apiKey)) throw new MissingApiKeyException();
        }

        public TopdeckTournament[]? GetTournaments(TopdeckTournamentRequest request)
        {
            var client = new WebClient();
            client.Headers.Add("Authorization", _apiKey);
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            byte[] serverData = client.UploadData(Routes.TournamentRoute, "POST", Encoding.UTF8.GetBytes(request.ToJson()));
            string serverJson = Encoding.UTF8.GetString(serverData);

            return JsonConvert.DeserializeObject<TopdeckTournament[]>(serverJson);
        }

        public TopdeckStanding[]? GetStandings(string tournamentId)
        {
            var client = new WebClient();
            client.Headers.Add("Authorization", _apiKey);
            byte[] serverData = client.DownloadData(Routes.StandingsRoute.Replace("{TID}",tournamentId));
            string serverJson = Encoding.UTF8.GetString(serverData);

            return JsonConvert.DeserializeObject<TopdeckStanding[]>(serverJson);
        }
    }
}
