using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeDeckInfo
    {
        public DeckItem[] Mainboard{ get; set; }
        public DeckItem[] Sideboard { get; set; }
        public MtgMeleeRoundInfo[] Rounds { get; set; }
    }
}
