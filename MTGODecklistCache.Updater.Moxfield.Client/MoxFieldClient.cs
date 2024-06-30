using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Tools;
using Newtonsoft.Json;
using System.Formats.Tar;
using System.Net;

namespace MTGODecklistCache.Updater.Moxfield.Client
{
    public class MoxfieldClient
    {
        public Deck GetDeck(string deckId)
        {
            string json;
            try
            {
                json = new WebClient().DownloadString(Constants.DeckApiPage.Replace("{deckId}", deckId));
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse wr && wr.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            dynamic deck = JsonConvert.DeserializeObject<dynamic>(json);

            List<DeckItem> maindeckCards = new List<DeckItem>();
            foreach (var card in deck.boards.mainboard.cards)
            {
                string name = card.First.card.name;
                int count = card.First.quantity;

                maindeckCards.Add(new DeckItem()
                {
                    CardName = CardNameNormalizer.Normalize(name),
                    Count = count
                }); ;
            }

            List<DeckItem> sideboardCards = new List<DeckItem>();
            foreach (var card in deck.boards.sideboard.cards)
            {
                string name = card.First.card.name;
                int count = card.First.quantity;

                sideboardCards.Add(new DeckItem()
                {
                    CardName = CardNameNormalizer.Normalize(name),
                    Count = count
                });
            }

            return new Deck()
            {
                AnchorUri = new Uri(Constants.DeckPage.Replace("{deckId}", deckId)),
                Mainboard = maindeckCards.ToArray(),
                Sideboard = sideboardCards.ToArray()
            };
        }
    }
}
