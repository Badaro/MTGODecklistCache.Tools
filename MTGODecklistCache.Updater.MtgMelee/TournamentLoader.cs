using HtmlAgilityPack;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Analyzer;
using MTGODecklistCache.Updater.MtgMelee.Client;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MTGODecklistCache.Updater.MtgMelee
{
    internal static class TournamentLoader
    {
        public static CacheItem GetTournamentDetails(MtgMeleeTournament tournament)
        {
            var players = new MtgMeleeClient().GetPlayers(tournament.Uri);

            List<Deck> decks = new List<Deck>();
            List<Standing> standings = new List<Standing>();
            Dictionary<string, Dictionary<string, RoundItem>> consolidatedRounds = new Dictionary<string, Dictionary<string, RoundItem>>();

            foreach (var player in players)
            {
                standings.Add(player.Standing);

                int playerPosition = player.Standing.Rank;
                string playerResult = playerPosition.ToString();
                if (playerPosition == 1) playerResult += "st Place";
                if (playerPosition == 2) playerResult += "nd Place";
                if (playerPosition == 3) playerResult += "rd Place";
                if (playerPosition > 3) playerResult += "th Place";

                var deck = GetDeck(player, players, tournament);
                if (deck != null) decks.Add(new Deck()
                {
                    AnchorUri = deck.DeckUri,
                    Mainboard = deck.Mainboard,
                    Sideboard = deck.Sideboard,
                    Player = player.PlayerName,
                    Date = null,
                    Result = playerResult
                });

                // Adds rounds to consolidated table, removing duplicates
                if (deck != null)
                {
                    foreach (var deckRound in deck.Rounds)
                    {
                        if (tournament.ExcludedRounds != null && tournament.ExcludedRounds.Contains(deckRound.RoundName)) continue;

                        if (!consolidatedRounds.ContainsKey(deckRound.RoundName)) consolidatedRounds.Add(deckRound.RoundName, new Dictionary<string, RoundItem>());
                        string roundItemKey = $"{deckRound.RoundName}_{deckRound.Match.Player1}_{deckRound.Match.Player2}";
                        if (!consolidatedRounds[deckRound.RoundName].ContainsKey(roundItemKey)) consolidatedRounds[deckRound.RoundName].Add(roundItemKey, deckRound.Match);
                    }
                }
            }

            var rounds = consolidatedRounds.Select(r => new Round() { RoundName = r.Key, Matches = r.Value.Select(m => m.Value).ToArray() }).ToArray();

            var bracket = new List<Round>();
            bracket.AddRange(rounds.Where(r => r.RoundName == "Quarterfinals"));
            bracket.AddRange(rounds.Where(r => r.RoundName == "Semifinals"));
            bracket.AddRange(rounds.Where(r => r.RoundName == "Finals"));

            if (bracket.Count() > 0)
            {
                decks = OrderNormalizer.ReorderDecks(decks.ToArray(), standings.ToArray(), bracket.ToArray()).ToList();
            }

            return new CacheItem()
            {
                Tournament = new Tournament(tournament),
                Decks = decks.ToArray(),
                Standings = standings.ToArray(),
                Rounds = rounds
            };
        }

        private static MtgMeleeDeckInfo GetDeck(MtgMeleePlayerInfo player, MtgMeleePlayerInfo[] players, MtgMeleeTournament tournament)
        {
            int currentPosition = 0;
            int bufferWidth = 80;

            Console.Write($"\r{new String(' ', bufferWidth)}");
            Console.Write($"\r[MtgMelee] Downloading player {player.PlayerName} ({++currentPosition})");

            Uri deckUri = null;
            if (player.DeckUris != null && player.DeckUris.Length > 0)
            {
                if (tournament.DeckOffset == null)
                {
                    deckUri = player.DeckUris.Last(); // Old behavior for compatibility reasons
                }
                else
                {
                    if (player.DeckUris.Length >= tournament.ExpectedDecks)
                    {
                        deckUri = player.DeckUris[tournament.DeckOffset.Value];
                    }
                    else
                    {
                        if (tournament.FixBehavior == MtgMeleeMissingDeckBehavior.UseLast)
                        {
                            deckUri = player.DeckUris.Last();
                        }
                    }
                }
            }

            if (deckUri != null)
            {
                return new MtgMeleeClient().GetDeck(deckUri, players);
            }
            else
            {
                return null;
            }
        }
    }
}