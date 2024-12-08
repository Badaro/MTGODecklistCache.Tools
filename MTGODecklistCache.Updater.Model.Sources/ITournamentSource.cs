using System;

namespace MTGODecklistCache.Updater.Model.Sources
{
    public interface ITournamentSource
    {
        string Provider { get; }
        Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null);
        CacheItem GetTournamentDetails(Tournament tournament);
    }
}