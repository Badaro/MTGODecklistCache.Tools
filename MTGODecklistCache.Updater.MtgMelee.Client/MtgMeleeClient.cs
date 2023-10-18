using HtmlAgilityPack;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MTGODecklistCache.Updater.MtgMelee.Client
{
    public class MtgMeleeClient
    {
        public MtgMeleeTournamentInfo GetTournament(Uri uri)
        {
            MtgMeleeTournamentInfo result = new MtgMeleeTournamentInfo();
            result.ID = int.Parse(uri.AbsolutePath.Split('/').Where(s => !String.IsNullOrEmpty(s)).Last());

            string pageContent = new WebClient().DownloadString(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);

            var tournamentInfoDiv = doc.DocumentNode.SelectSingleNode("//div[@id='tournament-headline-details-card']");
            var tournamentName = WebUtility.HtmlDecode(tournamentInfoDiv.SelectSingleNode("a/h3").InnerText.Trim());
            result.Name = tournamentName;

            var tournamentOrganizerLink = doc.DocumentNode.SelectSingleNode("//p[@id='tournament-headline-organization']/a");
            var tournamentOrganizer = WebUtility.HtmlDecode(tournamentOrganizerLink.InnerText.Trim());
            result.Organizer = tournamentOrganizer;

            var tournamentDate = doc.DocumentNode.SelectSingleNode("//p[@id='tournament-headline-start-date-field']/span").Attributes["data-value"].Value.Trim();
            result.Date = DateTime.Parse(tournamentDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();

            var tournamentRegistration = doc.DocumentNode.SelectSingleNode("//p[@id='tournament-headline-registration']").InnerText.Trim();
            result.Formats = tournamentRegistration.Split("|").FirstOrDefault(f => f.Contains("Format:")).Replace("Format:", "").Trim().Split(",").Select(f => f.Trim()).ToArray();

            var phaseNode = doc.DocumentNode.SelectNodes("//div[@id='standings-phase-selector-container']")?.First();

            if (phaseNode != null)
            {
                var phaseId = phaseNode.SelectNodes("button[@class='btn btn-primary round-selector']").Last().Attributes["data-id"].Value;

                string phaseParameters = MtgMeleeConstants.PhaseParameters.Replace("{start}", "0");
                string phaseUrl = MtgMeleeConstants.PhasePage.Replace("{phaseId}", phaseId);

                string json = Encoding.UTF8.GetString(new WebClient().UploadValues(phaseUrl, "POST", HttpUtility.ParseQueryString(phaseParameters)));
                var phase = JsonConvert.DeserializeObject<dynamic>(json);

                int maxDecklists = 0;
                foreach (var player in phase.data)
                {
                    int playerDecklists = player.Decklists.Count;
                    if (playerDecklists > maxDecklists) maxDecklists = playerDecklists;
                }
            }

            return result;
        }

        public MtgMeleePlayerInfo[] GetPlayers(Uri uri, int? maxPlayers = null)
        {
            List<MtgMeleePlayerInfo> result = new List<MtgMeleePlayerInfo>();

            string pageContent = new WebClient().DownloadString(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);

            var phaseNode = doc.DocumentNode.SelectNodes("//div[@id='standings-phase-selector-container']")?.First();
            if (phaseNode == null) return null;

            var phaseId = phaseNode.SelectNodes("button[@class='btn btn-primary round-selector']").Last().Attributes["data-id"].Value;

            bool hasData;
            int offset = 0;
            do
            {
                hasData = false;

                string phaseParameters = MtgMeleeConstants.PhaseParameters.Replace("{start}", offset.ToString());
                string phaseUrl = MtgMeleeConstants.PhasePage.Replace("{phaseId}", phaseId);

                string json = Encoding.UTF8.GetString(new WebClient().UploadValues(phaseUrl, "POST", HttpUtility.ParseQueryString(phaseParameters)));
                var phase = JsonConvert.DeserializeObject<dynamic>(json);

                foreach (var player in phase.data)
                {
                    hasData = true;
                    string playerName = player.Name;
                    playerName = NormalizeSpaces(playerName);
                    string userName = player.Username;

                    int playerPoints = player.Points;
                    double omwp = player.Tiebreaker1;
                    double gwp = player.Tiebreaker2;
                    double ogwp = player.Tiebreaker3;
                    int playerPosition = player.Rank;
                    string playerResult = player.Record;

                    Standing standing = new Standing()
                    {
                        Player = playerName,
                        Rank = playerPosition,
                        Points = playerPoints,
                        OMWP = omwp,
                        GWP = gwp,
                        OGWP = ogwp
                    };

                    List<string> playerDeckListIds = new List<string>();
                    foreach (var decklist in player.Decklists)
                    {
                        string deckListId = decklist.ID;
                        playerDeckListIds.Add(deckListId);
                    }

                    result.Add(new MtgMeleePlayerInfo()
                    {
                        UserName = userName,
                        PlayerName = playerName,
                        Result = playerResult,
                        Standing = standing,
                        DeckUris = playerDeckListIds.Count() == 0 ? null : playerDeckListIds.Select(id => new Uri(MtgMeleeConstants.DeckPage.Replace("{deckId}", id))).ToArray()
                    });
                }

                offset += 25;
            } while (hasData && (!maxPlayers.HasValue || offset < maxPlayers.Value));

            return result.ToArray();
        }

        public MtgMeleeDeckInfo GetDeck(Uri uri, MtgMeleePlayerInfo[] players, bool skipRoundData = false)
        {
            string deckPageContent = new WebClient().DownloadString(uri);

            HtmlDocument deckDoc = new HtmlDocument();
            deckDoc.LoadHtml(deckPageContent);

            var copyButton = deckDoc.DocumentNode.SelectSingleNode("//button[@class='decklist-builder-copy-button btn-sm btn btn-card text-nowrap ']");
            var cardList = WebUtility.HtmlDecode(copyButton.Attributes["data-clipboard-text"].Value).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var playerName = NormalizeSpaces(WebUtility.HtmlDecode(deckDoc.DocumentNode.SelectSingleNode("//span[@class='decklist-card-title-author']/a").InnerText));

            List<DeckItem> mainBoard = new List<DeckItem>();
            List<DeckItem> sideBoard = new List<DeckItem>();
            bool insideSideboard = false;
            bool insideCompanion = false;

            foreach (var card in cardList)
            {
                if (card == "Deck" || card == "Companion" || card == "Sideboard")
                {
                    if (card == "Companion")
                    {
                        insideCompanion = true;
                        insideSideboard = false;
                    }
                    else
                    {
                        if (card == "Sideboard")
                        {
                            insideCompanion = false;
                            insideSideboard = true;
                        }
                        else
                        {
                            insideCompanion = false;
                            insideSideboard = false;
                        }
                    }
                }
                else
                {
                    if (insideCompanion) continue; // Companion is listed in its own section *and* in the sideboard as well

                    int splitPosition = card.IndexOf(" ");
                    int count = Convert.ToInt32(new String(card.Take(splitPosition).ToArray()));
                    string name = new String(card.Skip(splitPosition + 1).ToArray());
                    name = CardNameNormalizer.Normalize(name);

                    if (insideSideboard) sideBoard.Add(new DeckItem() { CardName = name, Count = count });
                    else mainBoard.Add(new DeckItem() { CardName = name, Count = count });
                }
            }

            List<MtgMeleeRoundInfo> rounds = new List<MtgMeleeRoundInfo>();
            if (!skipRoundData)
            {
                var roundsDiv = deckDoc.DocumentNode.SelectSingleNode("//div[@id='tournament-path-grid-item']");
                if (roundsDiv != null)
                {
                    foreach (var roundDiv in roundsDiv.SelectNodes("div/div/div/table/tbody/tr"))
                    {
                        rounds.Add(ParseRoundNode(roundDiv, playerName, players));
                    }
                }
            }

            return new MtgMeleeDeckInfo()
            {
                DeckUri = uri,
                Mainboard = mainBoard.ToArray(),
                Sideboard = sideBoard.ToArray(),
                Rounds = rounds.Count() == 0 ? null : rounds.ToArray()
            };
        }

        private static MtgMeleeRoundInfo ParseRoundNode(HtmlNode roundNode, string playerName, MtgMeleePlayerInfo[] players)
        {
            var roundColumns = roundNode.SelectNodes("td");
            if (roundColumns.First().InnerText.Trim() == "No results found") return null;

            string roundName = roundColumns.First().InnerHtml;
            roundName = NormalizeSpaces(WebUtility.HtmlDecode(roundName));

            string roundOpponentId = roundColumns.Skip(1).First().SelectSingleNode("a")?.Attributes["href"].Value.Split("/").Last();
            string roundOpponentRaw = roundColumns.Skip(1).First().SelectSingleNode("a")?.InnerHtml;
            string roundOpponent = "-";
            if (roundOpponentId != null)
            {
                var opponent = players.FirstOrDefault(p => p.UserName == roundOpponentId);
                if (opponent != null)
                {
                    roundOpponent = opponent.PlayerName;
                }
                else
                {
                    if (roundOpponentRaw != null)
                    {
                        roundOpponent = NormalizeSpaces(WebUtility.HtmlDecode(roundOpponentRaw));
                    }
                }
            }

            string roundResult = roundColumns.Skip(3).First().InnerHtml;
            roundResult = NormalizeSpaces(WebUtility.HtmlDecode(roundResult));

            RoundItem item = null;
            if (roundResult.StartsWith($"{playerName} won", StringComparison.InvariantCultureIgnoreCase))
            {
                // Victory
                item = new RoundItem()
                {
                    Player1 = playerName,
                    Player2 = roundOpponent,
                    Result = roundResult.Split(" ").Last()
                };
            }
            if (roundResult.StartsWith($"{roundOpponent} won", StringComparison.InvariantCultureIgnoreCase))
            {
                // Defeat
                item = new RoundItem()
                {
                    Player1 = roundOpponent,
                    Player2 = playerName,
                    Result = roundResult.Split(" ").Last()
                };
            }
            if (roundResult.EndsWith("Draw"))
            {
                // Draw
                if (String.Compare(playerName, roundOpponent) < 0)
                {
                    item = new RoundItem()
                    {
                        Player1 = playerName,
                        Player2 = roundOpponent,
                        Result = roundResult.Split(" ").First()
                    };
                }
                else
                {
                    item = new RoundItem()
                    {
                        Player1 = roundOpponent,
                        Player2 = playerName,
                        Result = roundResult.Split(" ").First()
                    };
                }
            }
            if (roundResult.EndsWith(" bye"))
            {
                // Victory by bye
                item = new RoundItem()
                {
                    Player1 = playerName,
                    Player2 = "-",
                    Result = "2-0-0"
                };
            }
            if (roundResult.StartsWith($"{playerName} forfeited", StringComparison.InvariantCultureIgnoreCase))
            {
                // Victory by concession
                item = new RoundItem()
                {
                    Player1 = playerName,
                    Player2 = roundOpponent,
                    Result = "0-2-0"
                };
            }
            if (roundResult.StartsWith($"Not reported"))
            {
                // Missing data
                if (String.Compare(playerName, roundOpponent) < 0)
                {
                    item = new RoundItem()
                    {
                        Player1 = playerName,
                        Player2 = roundOpponent,
                        Result = "0-0-0"
                    };
                }
                else
                {
                    item = new RoundItem()
                    {
                        Player1 = roundOpponent,
                        Player2 = playerName,
                        Result = "0-0-0"
                    };
                }
            }

            if (item == null) throw new FormatException($"Cannot parse round data for player {playerName} and opponent {roundOpponent}");

            return new MtgMeleeRoundInfo
            {
                RoundName = roundName,
                Match = item
            };
        }

        private static string NormalizeSpaces(string data)
        {
            return Regex.Replace(data, "\\s+", " ").Trim();
        }
    }
}
