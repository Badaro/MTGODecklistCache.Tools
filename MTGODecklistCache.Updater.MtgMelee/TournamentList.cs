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
            if (endDate == null) endDate = DateTime.UtcNow.AddDays(1).Date;

            var result = new List<MtgMeleeTournament>();
            while (startDate < endDate)
            {
                var currentEndDate = startDate.AddDays(7);

                Console.Write($"\r[MtgMelee] Downloading tournaments from {startDate} to {currentEndDate}".PadRight(LogSettings.BufferWidth));

                var tournaments = new MtgMeleeClient().GetTournaments(startDate, currentEndDate);

                foreach (var tournament in tournaments)
                {
                    var meleeTournaments = new MtgMeleeAnalyzer().GetScraperTournaments(tournament);
                    if(meleeTournaments!=null) result.AddRange(meleeTournaments);
                }
                
                startDate = startDate.AddDays(7);
            }
            Console.WriteLine($"\r[MtgMelee] Download finished".PadRight(LogSettings.BufferWidth));

            return result.ToArray();
        }
    }
}
