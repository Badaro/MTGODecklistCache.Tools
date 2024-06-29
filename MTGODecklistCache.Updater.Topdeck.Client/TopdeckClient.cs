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

        public TopdeckListTournament[]? GetTournamentList(TopdeckTournamentRequest request)
        {
            byte[] serverData = GetClient().UploadData(Routes.TournamentRoute, "POST", Encoding.UTF8.GetBytes(request.ToJson()));
            return NormalizeArrayResult<TopdeckListTournament>(serverData);
        }

        public TopdeckTournament? GetTournament(string tournamentId)
        {
            byte[] serverData = GetClient().DownloadData(Routes.FullTournamentRoute.Replace("{TID}", tournamentId));
            return NormalizeResult<TopdeckTournament>(serverData);
        }

        public TopdeckTournamentInfo? GetTournamentInfo(string tournamentId)
        {
            byte[] serverData = GetClient().DownloadData(Routes.TournamentInfoRoute.Replace("{TID}", tournamentId));
            return NormalizeResult<TopdeckTournamentInfo>(serverData);
        }

        public TopdeckStanding[]? GetStandings(string tournamentId)
        {
            byte[] serverData = GetClient().DownloadData(Routes.StandingsRoute.Replace("{TID}", tournamentId));
            return NormalizeArrayResult<TopdeckStanding>(serverData);
        }

        public TopdeckRound[]? GetRounds(string tournamentId)
        {
            byte[] serverData = GetClient().DownloadData(Routes.RoundsRoute.Replace("{TID}", tournamentId));
            return NormalizeArrayResult<TopdeckRound>(serverData);
        }

        private WebClient GetClient()
        {
            var client = new WebClient();
            client.Headers.Add("Authorization", _apiKey);
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return client;
        }

        private T NormalizeResult<T>(byte[] jsonData) where T : NormalizableObject
        {
            string json = Encoding.UTF8.GetString(jsonData);
            var result = JsonConvert.DeserializeObject<T>(json);
            result.Normalize();
            return result;
        }

        private T[] NormalizeArrayResult<T>(byte[] jsonData) where T : NormalizableObject
        {
            string json = Encoding.UTF8.GetString(jsonData);
            var result = JsonConvert.DeserializeObject<List<T>>(json);
            result.ForEach(t => t.Normalize());
            return result.ToArray();
        }
    }
}
