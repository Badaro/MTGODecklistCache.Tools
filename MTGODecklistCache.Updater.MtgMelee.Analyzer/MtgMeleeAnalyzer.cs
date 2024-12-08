using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    public class MtgMeleeAnalyzer
    {
        public MtgMeleeTournament[] GetScraperTournaments(MtgMeleeListTournamentInfo tournament)
        {
            var tournamentInfo = new MtgMeleeClient().GetTournament(tournament.Uri);

            bool isProTour = tournament.Organizer == "Wizards of the Coast" && (tournament.Name.Contains("Pro Tour") || tournament.Name.Contains("World Championship")) && !tournament.Name.Contains("Qualifier");

            // Skips tournaments with blacklisted terms
            if (MtgMeleeAnalyzerSettings.BlacklistedTerms.Any(s => tournament.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase))) return null;

            // Skips tournaments with weird formats
            var validFormats = tournamentInfo.Formats.Where(f => MtgMeleeAnalyzerSettings.ValidFormats.Contains(f)).ToArray();
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

        private string GenerateFileName(MtgMeleeListTournamentInfo tournament, string format, int offset)
        {
            string name = tournament.Name;
            if (!name.Contains(format, StringComparison.InvariantCultureIgnoreCase)) name += $" ({format})";

            foreach (var otherFormat in MtgMeleeAnalyzerSettings.ValidFormats.Where(f => !f.Equals(format, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (name.Contains(otherFormat, StringComparison.InvariantCultureIgnoreCase)) name = name.Replace(otherFormat, otherFormat.Substring(0, 3), StringComparison.InvariantCultureIgnoreCase);
            }

            if (offset >= 0) name += $" (Seat {offset + 1})";

            return $"{SlugGenerator.SlugGenerator.GenerateSlug(name.Trim())}-{tournament.ID}-{tournament.Date.ToString("yyyy-MM-dd")}.json";
        }
    }
}
