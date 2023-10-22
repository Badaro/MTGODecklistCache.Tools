using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Analyzer;
using MTGODecklistCache.Updater.MtgMelee.Client;
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
                    var meleeTournaments = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);
                    if (meleeTournaments != null) result.AddRange(meleeTournaments);
                }

                startDate = startDate.AddDays(7);
            }
            Console.WriteLine($"\r[MtgMelee] Download finished".PadRight(LogSettings.BufferWidth));

            return result.ToArray();
        }
    }
}
