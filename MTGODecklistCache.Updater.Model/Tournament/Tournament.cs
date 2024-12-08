using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.Model
{
    public class Tournament
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        [JsonIgnore]
        public string JsonFile { get; set; }
        [JsonIgnore]
        public bool ForceRedownload { get; set; }
        [JsonIgnore]
        public string[] ExcludedRounds { get; set; }

        public Tournament()
        {
        }

        public Tournament(Tournament tournament)
        {
            this.Date = tournament.Date;
            this.Name = tournament.Name;
            this.Uri = tournament.Uri;
            this.JsonFile = tournament.JsonFile;
            this.ForceRedownload = tournament.ForceRedownload;
            this.ExcludedRounds = tournament.ExcludedRounds;
        }

        public override string ToString()
        {
            return this.Name + "|" + this.Date.ToString("yyyy-MM-dd");
        }
    }
}
