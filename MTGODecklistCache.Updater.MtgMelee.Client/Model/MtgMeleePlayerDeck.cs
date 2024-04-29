using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleePlayerDeck
    {
        public string ID { get; set; }
        public Uri Uri { get; set; }
        public string Format { get; set; }
    }
}
