using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;

namespace MTGODecklistCache.Updater.MtgMelee.Analyzer
{
    public class MtgMeleeAnalyzer
    {
        public MtgMeleeTournament[] GetScraperTournaments(MtgMeleeListTournamentInfo tournament)
        {
            var tournamentInfo = new MtgMeleeClient().GetTournament(tournament.Uri);

            bool isProTour = tournament.Organizer == "Wizards of the Coast" && (tournament.Name.Contains("Pro Tour") || tournament.Name.Contains("World Championship")) && !tournament.Name.Contains("Qualifier");

            // Skips tournaments with blacklisted terms
            if (MtgMeleeAnalyzerSettings.BlacklistedTerms.Any(s => tournament.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase))) return null;

            // Skips tournaments with weird formats
            if (!isProTour && tournamentInfo.Formats.Any(f => !MtgMeleeAnalyzerSettings.ValidFormats.Contains(f))) return null;

            // Skips small tournaments
            if (tournament.Decklists < MtgMeleeAnalyzerSettings.MinimumPlayers) return null;

            var players = new MtgMeleeClient().GetPlayers(tournamentInfo, MtgMeleeAnalyzerSettings.PlayersLoadedForAnalysis);

            // Skips empty tournaments
            if (players == null) return null;

            // Skips small tournaments
            if (players.Length < MtgMeleeAnalyzerSettings.MinimumPlayers) return null;

            // Skips "mostly empty" tournaments
            var totalPlayers = players.Length;
            var playersWithDecks = players.Where(p => p.Decks != null).Count();
            if (playersWithDecks < totalPlayers * MtgMeleeAnalyzerSettings.MininumPercentageOfDecks) return null;

            var maxDecksPerPlayer = players.Where(p => p.Decks != null).Max(p => p.Decks.Length);

            if (isProTour)
            {
                return new MtgMeleeTournament[]
                {
                    GenerateProTourTournament(tournament, players)
                };
            }
            else
            {
                if (maxDecksPerPlayer == 1)
                {
                    return new MtgMeleeTournament[]
                    {
                        GenerateSingleFormatTournament(tournament, tournamentInfo.Formats)
                    };
                }
                else
                {
                    List<MtgMeleeTournament> result = new List<MtgMeleeTournament>();
                    for (int i = 0; i < maxDecksPerPlayer; i++) result.Add(GenerateMultiFormatTournament(tournament, players, i, maxDecksPerPlayer));
                    return result.ToArray();
                }
            }
        }

        private MtgMeleeTournament GenerateSingleFormatTournament(MtgMeleeListTournamentInfo tournament, string[] formats)
        {
            return new MtgMeleeTournament()
            {
                Uri = tournament.Uri,
                Date = tournament.Date,
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, formats.First(), -1),
            };
        }

        private MtgMeleeTournament GenerateMultiFormatTournament(MtgMeleeListTournamentInfo tournament, MtgMeleePlayerInfo[] players, int offset, int expectedDecks)
        {
            Uri[] deckUris = players.Where(p => p.Decks != null && p.Decks.Length > offset).Select(p => p.Decks[offset].Uri).Take(MtgMeleeAnalyzerSettings.DecksLoadedForAnalysis).ToArray();
            MtgMeleeDeckInfo[] decks = deckUris.Select(d => new MtgMeleeClient().GetDeck(d, players, true)).ToArray();

            string format = FormatDetector.Detect(decks);

            return new MtgMeleeTournament()
            {
                Uri = tournament.Uri,
                Date = tournament.Date,
                Name = tournament.Name,
                JsonFile = FilenameGenerator.GenerateFileName(tournament.ID.Value.ToString(), tournament.Name, tournament.Date, format, MtgMeleeAnalyzerSettings.ValidFormats, offset),
                DeckOffset = offset,
                ExpectedDecks = expectedDecks,
                FixBehavior = MtgMeleeMissingDeckBehavior.Skip
            };
        }

        private MtgMeleeTournament GenerateProTourTournament(MtgMeleeListTournamentInfo tournament, MtgMeleePlayerInfo[] players)
        {
            Uri[] deckUris = players.Where(p => p.Decks != null).Select(p => p.Decks.Last().Uri).ToArray();
            MtgMeleeDeckInfo[] decks = deckUris.Select(d => new MtgMeleeClient().GetDeck(d, players, true)).ToArray();

            string format = FormatDetector.Detect(decks);

            return new MtgMeleeTournament()
            {
                Uri = tournament.Uri,
                Date = tournament.Date,
                Name = tournament.Name,
                JsonFile = GenerateFileName(tournament, format, -1),
                DeckOffset = 0,
                ExpectedDecks = 3,
                FixBehavior = MtgMeleeMissingDeckBehavior.UseFirst,
                ExcludedRounds = new string[] { "Round 1", "Round 2", "Round 3", "Round 9", "Round 10", "Round 11" }
            };
        }

        private string GenerateFileName(MtgMeleeListTournamentInfo tournament, string format, int offset)
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
