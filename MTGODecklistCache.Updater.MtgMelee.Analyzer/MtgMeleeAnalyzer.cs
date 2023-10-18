using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using System;
using System.ComponentModel.Design;
using System.Linq;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    public class MtgMeleeAnalyzer
    {
        static readonly string[] _knownFormats = new string[]
        {
            "Standard",
            "Modern",
            "Pioneer",
            "Legacy",
            "Vintage",
            "Pauper"
        };

        public MtgMeleeTournament[] GetScraperTournaments(Uri uri)
        {
            var tournament = new MtgMeleeClient().GetTournament(uri);
            var players = new MtgMeleeClient().GetPlayers(uri);

            if (tournament.Formats.Length == 1 && players.Where(p => p.DeckUris != null).Max(p => p.DeckUris.Length) == 1)
            {
                return new MtgMeleeTournament[]
                {
                    GenerateSingleFormatTournament(uri, tournament, players)
                };
            }

            throw new NotImplementedException();
        }

        private MtgMeleeTournament GenerateSingleFormatTournament(Uri uri, MtgMeleeTournamentInfo tournament, MtgMeleePlayerInfo[] players)
        {
            return new MtgMeleeTournament()
            {
                Uri = uri,
                Date = tournament.Date,
                Name = GenerateName(tournament.Name, tournament.Formats.First()),
            };
        }

        private string GenerateName(string name, string format)
        {
            if (!name.Contains(format, StringComparison.InvariantCultureIgnoreCase)) name += $" ({format})";

            foreach (var otherFormat in _knownFormats.Where(f => !f.Equals(format, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (name.Contains(otherFormat, StringComparison.InvariantCultureIgnoreCase)) name = name.Replace(otherFormat, otherFormat.Substring(0, 3), StringComparison.InvariantCultureIgnoreCase);
            }

            return name;
        }
    }
}
