using System;

namespace MTGODecklistCache.Updater.Model.Sources
{
    public interface ITournamentSource<T>
        where T : Tournament
    {
        string Provider { get; }
        T[] GetTournaments(DateTime startDate, DateTime? endDate = null);
        CacheItem GetTournamentDetails(T tournament);
    }
}