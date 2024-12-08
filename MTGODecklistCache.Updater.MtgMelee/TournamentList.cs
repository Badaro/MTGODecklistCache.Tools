using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    internal static class TournamentList
    {
        public static readonly string[] ValidFormats = new string[]
        {
            "Standard",
            "Modern",
            "Pioneer",
            "Legacy",
            "Vintage",
            "Pauper"
        };
        public static readonly string[] BlacklistedTerms = new string[]
        {
            "Team "
        };

        public static MtgMeleeTournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            if (startDate < new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc)) return new MtgMeleeTournament[0];
            if (endDate == null) endDate = DateTime.UtcNow.AddDays(1).Date;

            var result = new List<MtgMeleeTournament>();
            while (startDate < endDate)
            {
                var currentEndDate = startDate.AddDays(7);

                Console.Write($"\r[MtgMelee] Downloading tournaments from {startDate.ToString("yyyy-MM-dd")} to {currentEndDate.ToString("yyyy-MM-dd")}".PadRight(LogSettings.BufferWidth));

                var tournaments = new MtgMeleeClient().GetTournaments(startDate, currentEndDate);

                foreach (var tournament in tournaments)
                {
                    var meleeTournaments = GetScraperTournaments(tournament);
                    if (meleeTournaments != null) result.AddRange(meleeTournaments);
                }

                startDate = startDate.AddDays(7);
            }
            Console.WriteLine($"\r[MtgMelee] Download finished".PadRight(LogSettings.BufferWidth));

            return result.ToArray();
        }

        public static MtgMeleeTournament[] GetScraperTournaments(MtgMeleeListTournamentInfo tournament)
        {
            var tournamentInfo = new MtgMeleeClient().GetTournament(tournament.Uri);

            bool isProTour = tournament.Organizer == "Wizards of the Coast" && (tournament.Name.Contains("Pro Tour") || tournament.Name.Contains("World Championship")) && !tournament.Name.Contains("Qualifier");

            // Skips tournaments with blacklisted terms
            if (BlacklistedTerms.Any(s => tournament.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase))) return null;

            // Skips tournaments with weird formats
            var validFormats = tournamentInfo.Formats.Where(f => ValidFormats.Contains(f)).ToArray();
            if (validFormats.Length == 0) return null;

            var result = new MtgMeleeTournament()
            {
                Uri = tournament.Uri,
                Date = tournament.Date,
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, validFormats.First(), -1),
            };

            if (isProTour)
            {
                result.ExcludedRounds = new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" };
            }

            return new MtgMeleeTournament[] { result };
        }

        private static string GenerateFileName(MtgMeleeListTournamentInfo tournament, string format, int offset)
        {
            string name = tournament.Name;
            if (!name.Contains(format, StringComparison.InvariantCultureIgnoreCase)) name += $" ({format})";

            foreach (var otherFormat in ValidFormats.Where(f => !f.Equals(format, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (name.Contains(otherFormat, StringComparison.InvariantCultureIgnoreCase)) name = name.Replace(otherFormat, otherFormat.Substring(0, 3), StringComparison.InvariantCultureIgnoreCase);
            }

            if (offset >= 0) name += $" (Seat {offset + 1})";

            return $"{SlugGenerator.SlugGenerator.GenerateSlug(name.Trim())}-{tournament.ID}-{tournament.Date.ToString("yyyy-MM-dd")}.json";
        }
    }
}
