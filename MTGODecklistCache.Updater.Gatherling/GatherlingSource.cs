using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace MTGODecklistCache.Updater.Gatherling
{
    public class GatherlingSource : ITournamentSource
    {
        public string Provider { get { return "gatherlink.com"; } }

        private static string _eventPage = "https://gatherling.com/eventreport.php?event={eventName}";
        private static string _eventPageApi = "https://gatherling.com/api.php?action=eventinfo&event={eventName}";
        private static Dictionary<string, string> _validFormats = new Dictionary<string, string>()
        {
            { "Penny Dreadful", "penny dreadful" },
            { "Pre-FIRE", "prefire" },
            { "PreModern", "premod" }
        };
        private static Dictionary<string, string> _replacements = new Dictionary<string, string>()
        {
            { "Pre-FIRE", "prefire" },
            { "Pre FIRE", "prefire" },
            { "PreFIRE", "prefire" },
            { "Pre Modern", "premod" },
            { "Pre-Modern", "premod" },
            { "PreModern", "premod" },
            { "Penny Dreadful", "penny dreadful" },
            { "Penny-Dreadful", "penny dreadful" },
            { "PennyDreadful", "penny dreadful" },
            { "Modern", "mod" }
        };

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            string eventPageApi = _eventPageApi.Replace("{eventName}", HttpUtility.UrlEncode(tournament.Name));
            string jsonData = new WebClient().DownloadString(eventPageApi);

            dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonData);

            var result = new CacheItem();

            string error = json.error;
            if (error == "Event not found") return null;

            string eventName = json.name;
            string eventDate = json.start;
            string eventFormat = json.format;

            result.Tournament = new Tournament();
            result.Tournament.Name = eventName;
            result.Tournament.Uri = new Uri(_eventPage.Replace("{eventName}", eventName));
            result.Tournament.Date = DateTime.Parse(eventDate + "Z", CultureInfo.InvariantCulture).ToUniversalTime();
            result.Tournament.JsonFile = GenerateFileName(result.Tournament, eventFormat);

            return result;
        }

        public Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        private static string GenerateFileName(Tournament tournament, string format)
        {
            string name = tournament.Name;

            foreach (var replacement in _replacements)
            {
                if (name.Contains(replacement.Key, StringComparison.InvariantCultureIgnoreCase)) name = name.Replace(replacement.Key, replacement.Value, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!name.Contains(_validFormats[format], StringComparison.InvariantCultureIgnoreCase)) name += $" ({_validFormats[format]})";

            return $"{SlugGenerator.SlugGenerator.GenerateSlug(name.Trim())}-{tournament.Date.ToString("yyyy-MM-dd")}.json";
        }
    }
}
