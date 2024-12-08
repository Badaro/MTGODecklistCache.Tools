using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    public class MtgMeleeTournament : Tournament
    {
        public string[] ExcludedRounds { get; set; }
    }
}
