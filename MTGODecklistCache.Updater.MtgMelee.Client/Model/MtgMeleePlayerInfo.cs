using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleePlayerInfo
    {
        public string UserName { get; set; }
        public string PlayerName { get; set; }
        public string Result { get; set; }
        public Standing Standing { get; set; }
        public MtgMeleePlayerDeck[] Decks { get; set; }
    }
}
