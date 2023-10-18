using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using MTGODecklistCache.Updater.MtgMelee.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    public class MtgMeleeSource : ITournamentSource<MtgMeleeTournament>
    {
        public string Provider { get { return "melee.gg"; } }

        private string RawDataFolder { get; set; }

        public MtgMeleeSource(string rawDataFolder)
        {
            this.RawDataFolder = rawDataFolder;
        }

        public CacheItem GetTournamentDetails(MtgMeleeTournament tournament)
        {
            return TournamentLoader.GetTournamentDetails(tournament);
        }

        public MtgMeleeTournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            var result = TournamentList.GetTournaments<MtgMeleeTournament>(this.RawDataFolder);
            result = result.Where(t => t.Date >= startDate).ToArray();
            if (endDate != null) result = result.Where(t => t.Date <= endDate).ToArray();
            return result;
        }
    }
}
