using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeRoundInfo
    {
        public string RoundName { get; set; }
        public RoundItem Match { get; set; }
    }
}
