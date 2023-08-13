using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.ManaTraders
{
    public class ManaTradersSource : ITournamentSource<Tournament>
    {
        public string Provider { get { return "manatraders.com"; } }

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            return TournamentLoader.GetTournamentDetails(tournament);
        }

        public Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            var result = TournamentList.GetTournaments();
            result = result.Where(t => t.Date >= startDate).ToArray();
            if (endDate != null) result = result.Where(t => t.Date <= endDate).ToArray();
            return result;

        }
    }
}
