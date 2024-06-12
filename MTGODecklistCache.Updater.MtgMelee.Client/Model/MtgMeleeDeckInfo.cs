using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeDeckInfo
    {
        public Uri DeckUri { get; set; }
        public string Format { get; set; }
        public DeckItem[] Mainboard { get; set; }
        public DeckItem[] Sideboard { get; set; }
        public MtgMeleeRoundInfo[] Rounds { get; set; }
    }
}
