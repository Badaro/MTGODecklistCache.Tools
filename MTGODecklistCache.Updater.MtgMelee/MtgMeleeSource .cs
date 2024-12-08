using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    public class MtgMeleeSource : ITournamentSource<MtgMeleeTournament>
    {
        public string Provider { get { return "melee.gg"; } }

        public CacheItem GetTournamentDetails(MtgMeleeTournament tournament)
        {
            return TournamentLoader.GetTournamentDetails(tournament);
        }

        public MtgMeleeTournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            return TournamentList.GetTournaments(startDate, endDate);
        }
    }
}
