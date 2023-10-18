using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeDeckInfo
    {
        public Deck Deck { get; set; }
        public Standing Standing { get; set; }
        public Round[] Rounds { get; set; }
    }
}
