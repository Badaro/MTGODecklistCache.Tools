﻿using HtmlAgilityPack;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace MTGODecklistCache.Updater.Mtgo
{
    internal static class TournamentLoader
    {
        public static CacheItem GetTournamentDetails(Tournament tournament)
        {
            string htmlContent;
            using (WebClient client = new WebClient())
            {
                htmlContent = client.DownloadString(tournament.Uri);
            }

            var dataRow = htmlContent.Replace("\r", "").Split("\n").First(l => l.Trim().StartsWith("window.MTGO.decklists.data"));
            int cutStart = dataRow.IndexOf("{");

            var jsonData = dataRow.Substring(cutStart, dataRow.Length - cutStart - 1);
            dynamic json = JsonConvert.DeserializeObject(jsonData);

            var eventType = ParseEventType(json);

            Standing[] standing = null;
            if (eventType == "challenge" || eventType == "preliminary") standing = ParseStanding(json);

            Round[] bracket = null;
            if (eventType == "challenge") bracket = ParseBracket(json);

            Dictionary<int, string> winloss = null;
            if (eventType == "premilinary") winloss = ParseWinloss(json);

            var decks = ParseDecks(tournament, eventType, winloss, json);

            if (standing != null) decks = OrderNormalizer.ReorderDecks(decks, standing, bracket, bracket != null);

            return new CacheItem()
            {
                Standings = standing,
                Rounds = bracket,
                Decks = decks,
                Tournament = tournament
            };
        }

        private static string ParseEventType(dynamic json)
        {
            string eventType = "league";
            if (HasProperty(json, "standings"))
            {
                if (HasProperty(json, "final_rank"))
                {
                    eventType = "challenge";
                }
                else
                {
                    eventType = "preliminary";
                }
            }
            return eventType;
        }

        private static Dictionary<int, string> ParseWinloss(dynamic json)
        {
            Dictionary<int, string> playerWinloss = new Dictionary<int, string>();
            foreach (var winloss in json.winloss)
            {
                int playerid = winloss.loginid;
                int wins = winloss.wins;
                int losses = winloss.losses;

                playerWinloss.Add(playerid, $"{wins}-{losses}");
            }
            return playerWinloss;
        }

        private static Deck[] ParseDecks(Tournament tournament, string eventType, Dictionary<int, string> winloss, dynamic json)
        {
            DateTime eventDate = json.starttime;

            bool hasDecks = HasProperty(json, "decklists");
            if (!hasDecks) return null;

            HashSet<string> addedPlayers = new HashSet<string>();
            var decks = new List<Deck>();
            int rank = 1;
            foreach (var deck in json.decklists)
            {
                Dictionary<string, int> mainboard = new Dictionary<string, int>();
                Dictionary<string, int> sideboard = new Dictionary<string, int>();
                string player = deck.player;
                int playerId = deck.loginid;

                foreach (var card in deck.main_deck)
                {
                    string name = card.card_attributes.card_name;
                    int quantity = card.qty;

                    name = CardNameNormalizer.Normalize(name);

                    // JSON may contain multiple entries for the same card, likely if they come from different sets
                    if (!mainboard.ContainsKey(name)) mainboard.Add(name, 0);
                    mainboard[name] += quantity;
                }

                foreach (var card in deck.sideboard_deck)
                {
                    string name = card.card_attributes.card_name;
                    int quantity = card.qty;

                    name = CardNameNormalizer.Normalize(name);

                    // JSON may contain multiple entries for the same card, likely if they come from different sets
                    if (!sideboard.ContainsKey(name)) sideboard.Add(name, 0);
                    sideboard[name] += quantity;
                }

                var result = "";
                if (eventType == "preliminary")
                {
                    result = winloss.ContainsKey(playerId) ? winloss[playerId] : "";
                }
                if (eventType == "challenge")
                {
                    result = $"{rank}th Place";
                    if (rank == 1) result = "1st Place";
                    if (rank == 2) result = "2nd Place";
                    if (rank == 3) result = "3rd Place";
                    rank++;
                }
                if (eventType == "league")
                {
                    result = deck.wins.wins;
                    if (result == "5") result = "5-0";
                    if (result == "4") result = "4-1";
                    if (result == "3") result = "3-2";
                    if (result == "2") result = "2-4";
                    if (result == "1") result = "1-4";
                    if (result == "0") result = "0-5";
                }

                decks.Add(new Deck()
                {
                    AnchorUri = new Uri($"{tournament.Uri.ToString()}#deck_{player}"),
                    Date = eventDate,
                    Player = player,
                    Mainboard = mainboard.Select(k => new DeckItem() { CardName = k.Key, Count = k.Value }).ToArray(),
                    Sideboard = sideboard.Select(k => new DeckItem() { CardName = k.Key, Count = k.Value }).ToArray(),
                    Result = result
                });

                addedPlayers.Add(player);

            }

            return decks.ToArray();
        }

        private static Standing[] ParseStanding(dynamic json)
        {
            if (!HasProperty(json, "standings")) return null;

            List<Standing> standings = new List<Standing>();

            foreach (var standing in json.standings)
            {
                string player = standing.login_name;
                int points = standing.score;
                int rank = standing.rank;
                double GWP = standing.gamewinpercentage;
                double OGWP = standing.opponentgamewinpercentage;
                double OMWP = standing.opponentmatchwinpercentage;

                standings.Add(new Standing()
                {
                    Player = player,
                    Points = points,
                    Rank = rank,
                    GWP = GWP,
                    OGWP = OGWP,
                    OMWP = OMWP
                });
            }

            return standings.OrderBy(s => s.Rank).ToArray();
        }

        private static Round[] ParseBracket(dynamic json)
        {
            if (!HasProperty(json, "brackets")) return null;

            List<Round> brackets = new List<Round>();
            foreach (var bracket in json.brackets)
            {
                List<RoundItem> matches = new List<RoundItem>();

                foreach (var match in bracket.matches)
                {
                    string player1 = match.players[0].player;
                    string player2 = match.players[1].player;
                    int player1Wins = match.players[0].wins;
                    int player2Wins = match.players[1].wins;
                    bool reverseOrder = match.players[1].winner;

                    if (reverseOrder)
                    {
                        matches.Add(new RoundItem()
                        {
                            Player1 = player2,
                            Player2 = player1,
                            Result = $"{player2Wins}-{player1Wins}-0"
                        });
                    }
                    else
                    {
                        matches.Add(new RoundItem()
                        {
                            Player1 = player1,
                            Player2 = player2,
                            Result = $"{player1Wins}-{player2Wins}-0"
                        });
                    }
                }

                string roundName = "Quarterfinals";
                if (matches.Count == 2) roundName = "Semifinals";
                if (matches.Count == 1) roundName = "Finals";

                brackets.Add(new Round
                {
                    RoundName = roundName,
                    Matches = matches.ToArray()
                });
            }

            List<Round> result = new List<Round>();
            result.AddRange(brackets.Where(r => r.RoundName == "Quarterfinals"));
            result.AddRange(brackets.Where(r => r.RoundName == "Semifinals"));
            result.AddRange(brackets.Where(r => r.RoundName == "Finals"));

            return result.Count > 0 ? result.ToArray() : null;
        }

        private static bool HasProperty(dynamic obj, string name)
        {
            var jobj = (JObject)obj;
            return obj.Property(name) != null;
        }
    }
}
