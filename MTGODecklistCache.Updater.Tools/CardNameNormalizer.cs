using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace MTGODecklistCache.Updater.Tools
{
    public static class CardNameNormalizer
    {
        static readonly string _apiEndpoint = "https://api.scryfall.com/cards/search?order=cmc&q={query}";
        static readonly string _alchemyPrefix = "A-";
        static Dictionary<string, string> _normalization = new Dictionary<string, string>(StringComparer.InvariantCulture);

        static CardNameNormalizer()
        {
            AddTextReplacement("Aether -is:dfc", "Aether", "Æther");
            AddTextReplacement("Aether -is:dfc", "Aether", "Ã\u0086ther");
            AddMultinameCards("is:dfc -is:extra Aether", (f, b) => $"{f}", t => $"{t.Replace("Aether", "Æther")}", true);
            AddMultinameCards("is:dfc -is:extra Aether", (f, b) => $"{f}", t => $"{t.Replace("Aether", "Ã\u0086ther")}", true);

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

            // Wizards normalization errors
            //_normalization.Add("Lurrus of the Dream Den", "Lurrus of the Dream-Den");
            //_normalization.Add("Kongming, ??quot?Sleeping Dragon??quot?", "Kongming, \"Sleeping Dragon\"");
            //_normalization.Add("GhazbA?n Ogre", "Ghazbán Ogre");
            //_normalization.Add("Lim-DA?l's Vault", "Lim-Dûl's Vault");
            //_normalization.Add("Lim-DAul's Vault", "Lim-Dûl's Vault");
            //_normalization.Add("SAcance", "Séance");
            //_normalization.Add("JuzA?m Djinn", "Juzám Djinn");
            //_normalization.Add("Sol'kanar the Tainted", "Sol'Kanar the Tainted");
            //_normalization.Add("Minsc ?amp? Boo, Timeless Heroes", "Minsc & Boo, Timeless Heroes");

            // MTGO normalization errors
            //_normalization.Add("Jotun Grunt", "Jötun Grunt");
            //_normalization.Add("Lim-DÃ»l's Vault", "Lim-Dûl's Vault");
            //_normalization.Add("Rain Of Tears", "Rain of Tears");
            //_normalization.Add("Altar Of Dementia", "Altar of Dementia");
            //_normalization.Add("JuzÃ¡m Djinn", "Juzám Djinn");
            //_normalization.Add("SÃ©ance", "Séance");
            //_normalization.Add("Tura KennerÃ¼d, Skyknight", "Tura Kennerüd, Skyknight");
            //_normalization.Add("Name Sticker Goblin", "_____ Goblin");
            //_normalization.Add("BartolomÃ© del Presidio", "Bartolomé del Presidio");

            // MTGO normalization errors for LOTR
            //_normalization.Add("LÃ³rien Revealed", "Lórien Revealed");
            //_normalization.Add("Troll of Khazad-dÃ»m", "Troll of Khazad-dûm");
            //_normalization.Add("PalantÃ­r of Orthanc", "Palantír of Orthanc");
            //_normalization.Add("SmÃ©agol, Helpful Guide", "Sméagol, Helpful Guide");
            //_normalization.Add("Barad-dÃ»r", "Barad-dûr");
            //_normalization.Add("AndÃºril, Flame of the West", "Andúril, Flame of the West");
            //_normalization.Add("GrÃ­ma Wormtongue", "Gríma Wormtongue");
            //_normalization.Add("GrishnÃ¡kh, Brash Instigator", "Grishnákh, Brash Instigator");
            //_normalization.Add("Ã\u0089omer, Marshal of Rohan", "Éomer, Marshal of Rohan");
            //_normalization.Add("Ã\u0089omer of the Riddermark", "Éomer of the Riddermark");
            //_normalization.Add("DÃºnedain Blade", "Dúnedain Blade");
            //_normalization.Add("NazgÃ»l", "Nazgûl");
            //_normalization.Add("LothlÃ³rien Lookout", "Lothlórien Lookout");
            //_normalization.Add("Galadriel of LothlÃ³rien", "Galadriel of Lothlórien");
            //_normalization.Add("Arwen UndÃ³miel", "Arwen Undómiel");
            //_normalization.Add("DÃºnedain Rangers", "Dúnedain Rangers");
            //_normalization.Add("MauhÃºr, Uruk-hai Captain", "Mauhúr, Uruk-hai Captain");
            //_normalization.Add("Soothing of SmÃ©agol", "Soothing of Sméagol");
            //_normalization.Add("ThÃ©oden, King of Rohan", "Théoden, King of Rohan");
            //_normalization.Add("Tale of TinÃºviel", "Tale of Tinúviel");
            //_normalization.Add("Ã\u0089owyn, Lady of Rohan", "Éowyn, Lady of Rohan");
            //_normalization.Add("UglÃºk of the White Hand", "Uglúk of the White Hand");
            //_normalization.Add("Ãowyn, Fearless Knight", "Éowyn, Fearless Knight");

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

        private static void AddTextReplacement(string query, string validString, string invalidString)
        {
            string api = _apiEndpoint.Replace("{query}", WebUtility.UrlEncode(query));
            bool hasMore;
            do
            {
                string json = new WebClient().DownloadString(api);
                dynamic data = JsonConvert.DeserializeObject(json);

                foreach (var card in data.data)
                {
                    string cardName = card.name;
                    string invalidCardName = cardName.Replace(validString, invalidString);

                    if (!cardName.Contains(validString)) continue; // For some odd reason scryfall returns "Breya, Etherium Shaper" when searching for "Aether"
                    _normalization.Add(invalidCardName, cardName);
                }

                hasMore = data.has_more;
                api = data.next_page;
            }
            while (hasMore);
        }


        private static void AddMultinameCards(string criteria, Func<string, string, string> createTargetName, Func<string, string> textReplacement = null, bool onlyCombinedNames = false)
        {
            string api = _apiEndpoint.Replace("{query}", WebUtility.UrlEncode(criteria));
            bool hasMore;
            do
            {
                string json = new WebClient().DownloadString(api);
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
                string json = new WebClient().DownloadString(api);
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
                string json = new WebClient().DownloadString(api);
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
    }
}
