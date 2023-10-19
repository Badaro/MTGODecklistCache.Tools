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
        public MtgMeleeTournament[] GetScraperTournaments(Uri uri)
        {
            var tournament = new MtgMeleeClient().GetTournament(uri);
            var players = new MtgMeleeClient().GetPlayers(uri, 25);

            // Skips empty tournaments
            if (players == null) return null;

            // Skips small tournaments
            if (players.Length < MtgMeleeAnalyzerSettings.MinimumPlayers) return null;

            // Skips "mostly empty" tournaments
            var totalPlayers = players.Length;
            var playersWithDecks = players.Where(p => p.DeckUris != null).Count();
            if (playersWithDecks < totalPlayers * MtgMeleeAnalyzerSettings.MininumPercentageOfDecks) return null;

            bool isProTour = tournament.Organizer == "Wizards of the Coast" && tournament.Name.Contains("Pro Tour");

            // Skips tournaments with weird formats
            if (!isProTour && tournament.Formats.Any(f => !MtgMeleeAnalyzerSettings.ValidFormats.Contains(f))) return null;

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
                if (isProTour)
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
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, tournament.Formats.First(), -1),
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
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, format, offset),
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
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, format, -1),
                DeckOffset = 2,
                ExpectedDecks = 3,
                FixBehavior = MtgMeleeMissingDeckBehavior.UseLast,
                ExcludedRounds = new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" }
            };
        }

        private string GenerateFileName(MtgMeleeTournamentInfo tournament, string format, int offset)
        {
            string name = tournament.Name;
            if (!name.Contains(format, StringComparison.InvariantCultureIgnoreCase)) name += $" ({format})";

            foreach (var otherFormat in MtgMeleeAnalyzerSettings.ValidFormats.Where(f => !f.Equals(format, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (name.Contains(otherFormat, StringComparison.InvariantCultureIgnoreCase)) name = name.Replace(otherFormat, otherFormat.Substring(0, 3), StringComparison.InvariantCultureIgnoreCase);
            }

            if (offset >= 0) name += $" (Seat {offset + 1})";

            return $"{SlugGenerator.SlugGenerator.GenerateSlug(name.Trim())}-{tournament.ID}-{tournament.Date.ToString("yyyy-MM-dd")}.json";
        }
    }
}
