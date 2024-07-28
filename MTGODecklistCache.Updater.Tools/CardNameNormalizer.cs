using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MTGODecklistCache.Updater.Tools
{
    public static class CardNameNormalizer
    {
        static readonly string _apiEndpoint = "https://api.scryfall.com/cards/search?order=cmc&q={query}";
        static readonly string _alchemyPrefix = "A-";
        static Dictionary<string, string> _normalization = new Dictionary<string, string>(StringComparer.InvariantCulture);

        static CardNameNormalizer()
        {
            AddMultinameCards("is:split", (f, b) => $"{f} // {b}");
            AddMultinameCards("is:dfc -is:extra", (f, b) => $"{f}");
            AddMultinameCards("is:adventure", (f, b) => $"{f}");
            AddMultinameCards("is:flip", (f, b) => $"{f}");

            AddFlavorNames();

            // ManaTraders normalization errors
            _normalization.Add("Full Art Plains", "Plains");
            _normalization.Add("Full Art Island", "Island");
            _normalization.Add("Full Art Swamp", "Swamp");
            _normalization.Add("Full Art Mountain", "Mountain");
            _normalization.Add("Full Art Forest", "Forest");

            // MTGO normalization errors
            _normalization.Add("Altar Of Dementia", "Altar of Dementia");
            _normalization.Add("Rain Of Tears", "Rain of Tears");
            _normalization.Add("\"Name Sticker\" Goblin", "_____ Goblin");
            _normalization.Add("Jotun Grunt", "Jötun Grunt");
            _normalization.Add("Sol'kanar the Tainted", "Sol'Kanar the Tainted");
            _normalization.Add("Furnace Of Rath", "Furnace of Rath");

            // Melee.gg normalization errors
            _normalization.Add("\"Magnifying Glass Enthusiast\"", "Jacob Hauken, Inspector");
            _normalization.Add("\"Voltaic Visionary\"", "Voltaic Visionary");
            _normalization.Add("Goblin", "_____ Goblin");
        }

        public static string Normalize(string card)
        {
            card = card.Trim();
            if (card.StartsWith(_alchemyPrefix)) card = card.Substring(_alchemyPrefix.Length);
            if (_normalization.ContainsKey(card)) card = _normalization[card];
            return card;
        }

        private static void AddMultinameCards(string criteria, Func<string, string, string> createTargetName, Func<string, string> textReplacement = null, bool onlyCombinedNames = false)
        {
            string api = _apiEndpoint.Replace("{query}", WebUtility.UrlEncode(criteria));
            bool hasMore;
            do
            {
                string json = GetScryfallClient().DownloadString(api);
                dynamic data = JsonConvert.DeserializeObject(json);

                foreach (var card in data.data)
                {
                    // FDB: Sometimes happens during spoiler season there's a DFC card "partially" 
                    //      added with only one face known, those need to be skipped
                    if (!(card as JObject).ContainsKey("card_faces")) continue;

                    string front = card.card_faces[0].name;
                    string back = card.card_faces[1].name;

                    if (front == back) continue;

                    if (textReplacement != null) front = textReplacement(front);
                    if (textReplacement != null) back = textReplacement(back);

                    string target = createTargetName(front, back);

                    if (!onlyCombinedNames) _normalization.Add(front, target);
                    _normalization.Add($"{front}/{back}", target);
                    _normalization.Add($"{front} / {back}", target);
                    _normalization.Add($"{front}//{back}", target);
                    _normalization.Add($"{front} // {back}", target);
                    _normalization.Add($"{front}///{back}", target);
                    _normalization.Add($"{front} /// {back}", target);
                }

                hasMore = data.has_more;
                api = data.next_page;
            }
            while (hasMore);
        }

        private static void AddFlavorNames()
        {
            string api = _apiEndpoint.Replace("{query}", WebUtility.UrlEncode("has:flavorname -is:dfc"));
            bool hasMore;
            do
            {
                string json = GetScryfallClient().DownloadString(api);
                dynamic data = JsonConvert.DeserializeObject(json);

                foreach (var card in data.data)
                {
                    string name = card.name;
                    string flavorName = card.flavor_name;
                    if (!_normalization.ContainsKey(flavorName)) _normalization.Add(flavorName, name);
                }

                hasMore = data.has_more;
                api = data.next_page;
            }
            while (hasMore);

            api = _apiEndpoint.Replace("{query}", WebUtility.UrlEncode("has:flavorname is:dfc"));
            do
            {
                string json = GetScryfallClient().DownloadString(api);
                dynamic data = JsonConvert.DeserializeObject(json);

                foreach (var card in data.data)
                {
                    // FDB: Sometimes happens during spoiler season there's a DFC card "partially" 
                    //      added with only one face known, those need to be skipped
                    if (!(card as JObject).ContainsKey("card_faces")) continue;

                    string front = card.card_faces[0].name;
                    string back = card.card_faces[1].name;
                    string front_flavor = card.card_faces[0].flavor_name;
                    string back_flavor = card.card_faces[1].flavor_name;

                    if (front == back) continue;

                    _normalization.Add($"{front_flavor}", front);
                    _normalization.Add($"{front_flavor}/{back_flavor}", front);
                    _normalization.Add($"{front_flavor} / {back_flavor}", front);
                    _normalization.Add($"{front_flavor}//{back_flavor}", front);
                    _normalization.Add($"{front_flavor} // {back_flavor}", front);
                    _normalization.Add($"{front_flavor}///{back_flavor}", front);
                    _normalization.Add($"{front_flavor} /// {back_flavor}", front);
                }

                hasMore = data.has_more;
                api = data.next_page;
            }
            while (hasMore);
        }

        private static WebClient GetScryfallClient()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0");
            return client;
        }
    }
}
