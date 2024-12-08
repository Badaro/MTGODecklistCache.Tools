using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Model
{
    public class MtgMeleeListTournamentInfo
    {
        public int? ID { get; set; }
        public Uri Uri { get; set; }
        public DateTime Date { get; set; }
        public string Organizer { get; set; }
        public string Name { get; set; }
        public int? Decklists { get; set; }
        public string[]? Formats { get; set; }
    }
}
