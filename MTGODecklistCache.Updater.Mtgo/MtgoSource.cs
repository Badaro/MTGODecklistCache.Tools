using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo
{
    public class MtgoSource : ITournamentSource<Tournament>
    {
        public string Provider { get { return "mtgo.com"; } }

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            return TournamentLoader.GetTournamentDetails(tournament);
        }

        public Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            return TournamentList.GetTournaments(startDate, endDate);
        }
    }
}
