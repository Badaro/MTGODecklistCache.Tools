using MTGODecklistCache.Updater.Model.Sources;
using MTGODecklistCache.Updater.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTGODecklistCache.Updater.Topdeck.Client;
using MTGODecklistCache.Updater.Tools;
using MTGODecklistCache.Updater.Topdeck.Client.Constants;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Net;

namespace MTGODecklistCache.Updater.Topdeck
{
    public class TopdeckSource : ITournamentSource
    {
        public string Provider { get { return "topdeck.gg"; } }

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            string tournamentId = tournament.Uri.ToString().Substring(tournament.Uri.ToString().LastIndexOf("/") + 1);

            TopdeckClient client = new TopdeckClient();

            var tournamentData = client.GetTournament(tournamentId);
            var tournamentDataFromList = client.GetTournamentList(new Client.Model.TopdeckTournamentRequest()
            {
                Start = tournamentData.Data.StartDate,
                End = tournamentData.Data.StartDate + 1,
                Game = tournamentData.Data.Game.Value,
                Format = tournamentData.Data.Format.Value,
                Columns = new PlayerColumn[] { PlayerColumn.Name, PlayerColumn.Wins, PlayerColumn.Losses, PlayerColumn.Draws, PlayerColumn.DeckSnapshot }
            }).First(t => t.Name == tournamentData.Data.Name);

            List<Round> rounds = new List<Round>();
            foreach (var round in tournamentData.Rounds)
            {
                List<RoundItem> roundItems = new List<RoundItem>();
                foreach (var table in round.Tables)
                {
                    if (table.Winner == Misc.DrawText || table.Players.Length == 1)
                    {
                        if (table.Players.Length == 1)
                        {
                            // Byes
                            roundItems.Add(new RoundItem()
                            {
                                Player1 = table.Players[0].Name,
                                Player2 = String.Empty,
                                Result = "0-0-1"
                            });
                        }
                        else
                        {
                            roundItems.Add(new RoundItem()
                            {
                                Player1 = table.Players[0].Name,
                                Player2 = table.Players[1].Name,
                                Result = "0-0-1"
                            });
                        }
                    }
                    else
                    {
                        if (table.Winner == table.Players[0].Name)
                        {
                            roundItems.Add(new RoundItem()
                            {
                                Player1 = table.Players[0].Name,
                                Player2 = table.Players[1].Name,
                                Result = "1-0-0"
                            });
                        }
                        else
                        {
                            if (table.Winner == table.Players[1].Name)
                            {
                                roundItems.Add(new RoundItem()
                                {
                                    Player1 = table.Players[1].Name,
                                    Player2 = table.Players[0].Name,
                                    Result = "1-0-0"
                                });
                            }
                        }
                    }
                }
                rounds.Add(new Round()
                {
                    RoundName = round.Name,
                    Matches = roundItems.ToArray()
                });
            }

            List<Standing> standings = new List<Standing>();
            foreach (var standing in tournamentData.Standings)
            {
                var listStanding = tournamentDataFromList.Standings.FirstOrDefault(s => s.Name == standing.Name);

                standings.Add(new Standing()
                {
                    Player = standing.Name,
                    Rank = standing.Standing.Value,
                    Wins = listStanding.Wins.Value,
                    Losses = listStanding.Losses.Value,
                    Draws = listStanding.Draws.Value,
                    GWP = standing.GameWinRate.Value,
                    OMWP = standing.OpponentWinRate.Value,
                    OGWP = standing.OpponentGameWinRate.Value
                });
            }

            List<Deck> decks = new List<Deck>();
            foreach (var standing in tournamentData.Standings)
            {
                var listStanding = tournamentDataFromList.Standings.FirstOrDefault(s => s.Name == standing.Name);

                if (listStanding.DeckSnapshot != null && listStanding.DeckSnapshot.Mainboard != null)
                {
                    string playerResult = standing.Standing.ToString();
                    if (standing.Standing == 1) playerResult += "st Place";
                    if (standing.Standing == 2) playerResult += "nd Place";
                    if (standing.Standing == 3) playerResult += "rd Place";
                    if (standing.Standing > 3) playerResult += "th Place";

                    List<DeckItem> mainboard = null;
                    List<DeckItem> sideboard = null;

                    if(listStanding.DeckSnapshot!=null)
                    {
                        mainboard = new List<DeckItem>();
                        foreach (var card in listStanding.DeckSnapshot.Mainboard)
                        {
                            mainboard.Add(new DeckItem()
                            {
                                Count = card.Value,
                                CardName = CardNameNormalizer.Normalize(card.Key)
                            });
                        }

                        sideboard = new List<DeckItem>();
                        if (listStanding.DeckSnapshot.Sideboard != null)
                        {
                            foreach (var card in listStanding.DeckSnapshot.Sideboard)
                            {
                                sideboard.Add(new DeckItem()
                                {
                                    Count = card.Value,
                                    CardName = CardNameNormalizer.Normalize(card.Key)
                                });
                            }
                        }
                    }

                    decks.Add(new Deck()
                    {
                        Player = standing.Name,
                        Date = tournament.Date,
                        Result = playerResult,
                        AnchorUri = new Uri(standing.Decklist),
                        Mainboard = mainboard.ToArray(),
                        Sideboard = sideboard.ToArray()
                    });
                }
            }

            return new CacheItem()
            {
                Tournament = tournament,
                Standings = standings.ToArray(),
                Rounds = rounds.ToArray(),
                Decks = decks.ToArray()
            };
        }

        public Tournament[] GetTournaments(DateTime startDate, DateTime? endDate = null)
        {
            if (startDate < new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc)) return new Tournament[0];
            if (endDate == null) endDate = DateTime.UtcNow.AddDays(1).Date;

            Format[] validFormats = new Format[]
            {
                Format.Standard,
                Format.Pioneer,
                Format.Modern,
                Format.Legacy,
                Format.Vintage,
                Format.Pauper
            };

            var result = new List<Tournament>();
            while (startDate < endDate)
            {
                var currentEndDate = startDate.AddDays(7);

                Console.Write($"\r[Topdeck] Downloading tournaments from {startDate.ToString("yyyy-MM-dd")} to {currentEndDate.ToString("yyyy-MM-dd")}".PadRight(LogSettings.BufferWidth));

                foreach (var format in validFormats)
                {
                    var tournaments = new TopdeckClient().GetTournamentList(new()
                    {
                        Start = new DateTimeOffset(startDate, TimeSpan.Zero).ToUnixTimeSeconds(),
                        End = new DateTimeOffset(currentEndDate, TimeSpan.Zero).ToUnixTimeSeconds(),
                        Game = Game.MagicTheGathering,
                        Format = format
                    });

                    foreach (var tournament in tournaments)
                    {
                        var date = DateTimeOffset.FromUnixTimeSeconds(tournament.StartDate.Value).UtcDateTime;
                        result.Add(new Tournament()
                        {
                            Name = tournament.Name,
                            Date = date,
                            Uri = new Uri($"https://topdeck.gg/event/{tournament.ID}"),
                            JsonFile = FilenameGenerator.GenerateFileName(tournament.ID, tournament.Name, date, format.ToString(), validFormats.Select(f => f.ToString()).ToArray(), -1)
                        });
                    }
                }

                startDate = startDate.AddDays(7);
            }
            Console.WriteLine($"\r[Topdeck] Download finished".PadRight(LogSettings.BufferWidth));

            return result.ToArray();
        }
    }
}
