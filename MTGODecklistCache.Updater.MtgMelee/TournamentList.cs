using MTGODecklistCache.Updater.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    internal static class TournamentList
    {
        public static T[] GetTournaments<T>(string rawDataFolder) where T : Tournament
        {
            if (!Directory.Exists(rawDataFolder)) return new T[0];

            List<T> tournaments = new List<T>();
            foreach (string tournamentFile in Directory.GetFiles(rawDataFolder, "*.json", SearchOption.AllDirectories))
            {
                T tournament = JsonConvert.DeserializeObject<T>(File.ReadAllText(tournamentFile));
                tournament.Date = tournament.Date.ToUniversalTime();
                tournament.JsonFile = GenerateTournamentFile(tournament.Name, tournament.Date, tournament.Uri);
                tournaments.Add(tournament);
            }

            return tournaments.ToArray();
        }

        private static string GenerateTournamentFile(string tournamentName, DateTime tournamentDate, Uri tournamentUri)
        {
            string tournamentId = tournamentUri.AbsoluteUri.Split('/').Where(s => s.Length > 0).Last();
            if (tournamentName.Contains("Legacy European")) tournamentName = tournamentName.Replace("Legacy European", "LE");
            return $"{SlugGenerator.SlugGenerator.GenerateSlug(tournamentName.Trim())}-{tournamentId}-{tournamentDate.ToString("yyyy-MM-dd")}.json";
        }
    }
}
