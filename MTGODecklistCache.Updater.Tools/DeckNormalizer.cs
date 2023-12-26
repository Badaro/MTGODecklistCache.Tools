using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGODecklistCache.Updater.Tools
{
    public static class DeckNormalizer
    {
        public static Deck Normalize(Deck deck)
        {
            deck.Mainboard = CombineDuplicates(deck.Mainboard).OrderBy(c => c.CardName).ToArray();
            deck.Sideboard = CombineDuplicates(deck.Sideboard).OrderBy(c => c.CardName).ToArray();
            return deck;
        }

        private static DeckItem[] CombineDuplicates(DeckItem[] input)
        {
            Dictionary<string, DeckItem> combined = new Dictionary<string, DeckItem>();
            foreach (var item in input)
            {
                if (!combined.ContainsKey(item.CardName)) combined.Add(item.CardName, new DeckItem() { CardName = item.CardName, Count = 0 });
                combined[item.CardName].Count += item.Count;
            }
            return combined.Values.ToArray();
        }
    }
}
