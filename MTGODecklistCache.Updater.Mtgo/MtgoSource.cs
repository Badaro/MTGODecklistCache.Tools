using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.Mtgo
{
    public class MtgoSource : ITournamentSource
    {
        public string Provider { get { return "mtgo.com_limited_data"; } }

        private bool _includeLeagues;

        public MtgoSource(bool includeLeagues = true)
        {
            _includeLeagues = includeLeagues;
        }

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            return TournamentLoader.GetTournamentDetails(tournament);
        }

        public Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            var result = TournamentList.GetTournaments(startDate, endDate);
            if (!_includeLeagues) result = result.Where(r => !r.JsonFile.Contains("league")).ToArray();
            return result;
        }
    }
}
