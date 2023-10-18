using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    public class MtgMeleeAnalyzer
    {
        static readonly double _mininumDeckPercent = 0.5;
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
            var players = new MtgMeleeClient().GetPlayers(uri, 25);

            // Skips empty tournaments
            if (players == null) return null;

            // Skips "mostly empty" tournaments
            var totalPlayers = players.Length;
            var playersWithDecks = players.Where(p => p.DeckUris != null).Count();
            if (playersWithDecks < totalPlayers * _mininumDeckPercent) return null;

            var maxDecksPerPlayer = players.Where(p => p.DeckUris != null).Max(p => p.DeckUris.Length);

            if (maxDecksPerPlayer == 1)
            {
                return new MtgMeleeTournament[]
                {
                    GenerateSingleFormatTournament(uri, tournament)
                };
            }
            else
            {
                if (tournament.Organizer == "Wizards of the Coast" && tournament.Name.Contains("Pro Tour"))
                {
                    return new MtgMeleeTournament[]
                    {
                        GenerateProTourTournament(uri, tournament, players)
                    };
                }
                else
                {

                    List<MtgMeleeTournament> result = new List<MtgMeleeTournament>();
                    for (int i = 0; i < maxDecksPerPlayer; i++) result.Add(GenerateMultiFormatTournament(uri, tournament, players, i, maxDecksPerPlayer));
                    return result.ToArray();
                }
            }
        }

        private MtgMeleeTournament GenerateSingleFormatTournament(Uri uri, MtgMeleeTournamentInfo tournament)
        {
            return new MtgMeleeTournament()
            {
                Uri = uri,
                Date = tournament.Date,
                Name = GenerateName(tournament.Name, tournament.Formats.First()),
            };
        }

        private MtgMeleeTournament GenerateMultiFormatTournament(Uri uri, MtgMeleeTournamentInfo tournament, MtgMeleePlayerInfo[] players, int offset, int expectedDecks)
        {
            Uri[] deckUris = players.Where(p => p.DeckUris != null && p.DeckUris.Length > offset).Select(p => p.DeckUris[offset]).ToArray();
            MtgMeleeDeckInfo[] decks = deckUris.Select(d => new MtgMeleeClient().GetDeck(d, players, true)).ToArray();

            string format = FormatDetector.Detect(decks);

            return new MtgMeleeTournament()
            {
                Uri = uri,
                Date = tournament.Date,
                Name = GenerateName(tournament.Name, format),
                DeckOffset = offset,
                ExpectedDecks = expectedDecks,
                FixBehavior = MtgMeleeMissingDeckBehavior.Skip
            };
        }

        private MtgMeleeTournament GenerateProTourTournament(Uri uri, MtgMeleeTournamentInfo tournament, MtgMeleePlayerInfo[] players)
        {
            Uri[] deckUris = players.Where(p => p.DeckUris != null).Select(p => p.DeckUris.Last()).ToArray();
            MtgMeleeDeckInfo[] decks = deckUris.Select(d => new MtgMeleeClient().GetDeck(d, players, true)).ToArray();

            string format = FormatDetector.Detect(decks);

            return new MtgMeleeTournament()
            {
                Uri = uri,
                Date = tournament.Date,
                Name = GenerateName(tournament.Name, format),
                DeckOffset = 2,
                ExpectedDecks = 3,
                FixBehavior = MtgMeleeMissingDeckBehavior.UseLast,
                ExcludedRounds = new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" }
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
