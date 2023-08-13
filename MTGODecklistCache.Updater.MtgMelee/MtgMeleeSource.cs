using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using MTGODecklistCache.Updater.MtgMelee.Model;
using System;
using System.Collections.Generic;
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
            return TournamentList.GetTournaments<MtgMeleeTournament>(this.RawDataFolder);
        }
    }
}
