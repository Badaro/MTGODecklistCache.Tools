using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.Moxfield.Client
{
    public static class Constants
    {
        public static readonly string DeckPage = "https://www.moxfield.com/decks/{deckId}";
        public static readonly string DeckApiPage = "https://api2.moxfield.com/v3/decks/all/{deckId}";
    }
}
