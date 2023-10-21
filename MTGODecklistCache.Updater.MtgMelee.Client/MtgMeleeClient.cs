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
        public MtgMeleePlayerInfo[] GetPlayers(Uri uri, int? maxPlayers = null)
        {
            List<MtgMeleePlayerInfo> result = new List<MtgMeleePlayerInfo>();

            string pageContent = new WebClient().DownloadString(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);

            var phaseNode = doc.DocumentNode.SelectNodes("//div[@id='standings-phase-selector-container']")?.First();
            if (phaseNode == null) return null;

            var phaseIdNode = phaseNode.SelectNodes("button[@class='btn btn-primary round-selector']");
            if (phaseIdNode == null) return null;

            var phaseId = phaseIdNode.Last().Attributes["data-id"].Value;

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
                    if (playerName == null) continue;

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

            string playerUrl = deckDoc.DocumentNode.SelectSingleNode("//span[@class='decklist-card-title-author']/a")?.Attributes["href"]?.Value;
            string playerRaw = deckDoc.DocumentNode.SelectSingleNode("//span[@class='decklist-card-title-author']/a")?.InnerHtml;

            var playerName = GetPlayerName(playerRaw, playerUrl, players);

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
                        var round = GetRound(roundDiv, playerName, players);
                        if (round != null) rounds.Add(round);
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

        private static MtgMeleeRoundInfo GetRound(HtmlNode roundNode, string playerName, MtgMeleePlayerInfo[] players)
        {
            var roundColumns = roundNode.SelectNodes("td");
            if (roundColumns.First().InnerText.Trim() == "No results found") return null;

            string roundName = roundColumns.First().InnerHtml;
            roundName = NormalizeSpaces(WebUtility.HtmlDecode(roundName));

            string roundOpponentUrl = roundColumns.Skip(1).First().SelectSingleNode("a")?.Attributes["href"]?.Value;
            string roundOpponentRaw = roundColumns.Skip(1).First().SelectSingleNode("a")?.InnerHtml;
            string roundOpponent = GetPlayerName(roundOpponentRaw, roundOpponentUrl, players);

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
            if (roundResult.StartsWith("won "))
            {
                // Victory by unknown opponent
                item = new RoundItem()
                {
                    Player1 = "-",
                    Player2 = playerName,
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

        public MtgMeleeTournamentInfo[] GetTournaments(DateTime startDate, DateTime endDate)
        {
            int offset = 0;
            int limit = -1;

            List<MtgMeleeTournamentInfo> result = new List<MtgMeleeTournamentInfo>();
            do
            {
                string tournamentListParameters = MtgMeleeConstants.TournamentListParameters
                    .Replace("{offset}", offset.ToString())
                    .Replace("{startDate}", startDate.ToString("yyyy-MM-dd"))
                    .Replace("{endDate}", endDate.ToString("yyyy-MM-dd"));
                string tournamentListUrl = MtgMeleeConstants.TournamentListPage;

                string json = Encoding.UTF8.GetString(new WebClient().UploadValues(tournamentListUrl, "POST", HttpUtility.ParseQueryString(tournamentListParameters)));
                var list = JsonConvert.DeserializeObject<dynamic>(json);

                limit = list.recordsTotal;

                foreach (var item in list.data)
                {
                    offset++;

                    int id = item.ID;
                    DateTime date = item.StartDate;
                    string name = item.Name;
                    string organization = item.OrganizationName;
                    string format = item.FormatDescription;
                    string status = item.StatusDescription;
                    int decklists = item.Decklists;

                    name = NormalizeSpaces(name);
                    organization = NormalizeSpaces(organization);
                    format = NormalizeSpaces(format);
                    status = NormalizeSpaces(status);

                    if (status == "Ended" || (DateTime.Now - date).TotalDays > MtgMeleeConstants.MaxDaysBeforeTournamentMarkedAsEnded)
                    {
                        result.Add(new MtgMeleeTournamentInfo()
                        {
                            ID = id,
                            Date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc),
                            Name = name,
                            Organizer = organization,
                            Formats = new string[] { format },
                            Uri = new Uri(MtgMeleeConstants.TournamentPage.Replace("{tournamentId}", id.ToString())),
                            Decklists = decklists
                        });
                    }
                }
            } while (offset < limit);

            return result.ToArray();
        }

        private static string GetPlayerName(string playerNameRaw, string profileUrl, MtgMeleePlayerInfo[] players)
        {
            string playerId = profileUrl?.Split("/").Last();
            if (playerId != null)
            {
                var playerInfo = players.FirstOrDefault(p => p.UserName == playerId);
                if (playerInfo != null)
                {
                    return playerInfo.PlayerName;
                }
                else
                {
                    if (playerNameRaw != null)
                    {
                        return NormalizeSpaces(WebUtility.HtmlDecode(playerNameRaw));
                    }
                }
            }
            return "-";
        }

        private static string NormalizeSpaces(string data)
        {
            return Regex.Replace(data, "\\s+", " ").Trim();
        }
    }
}
