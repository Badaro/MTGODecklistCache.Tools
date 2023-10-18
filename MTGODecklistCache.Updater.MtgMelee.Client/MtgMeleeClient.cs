using HtmlAgilityPack;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model.Obsolte;
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
        public MtgMeleeTournament GetTournament(Uri uri)
        {

            MtgMeleeTournament result = new MtgMeleeTournament();
            result.ID = int.Parse(uri.AbsolutePath.Split('/').Where(s => !String.IsNullOrEmpty(s)).Last());

            string pageContent = new WebClient().DownloadString(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);

            var tournamentInfoDiv = doc.DocumentNode.SelectSingleNode("//div[@id='tournament-headline-details-card']");
            var tournamentName = WebUtility.HtmlDecode(tournamentInfoDiv.SelectSingleNode("a/h3").InnerText.Trim());
            result.Name = tournamentName;

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

                result.MaxDecklists = maxDecklists;
            }
            else
            {
                result.MaxDecklists = 0;
            }

            return result;
        }

        public MtgMeleePlayerInfo[] GetPlayers(Uri uri, int? maxPlayers = null)
        {
            List<MtgMeleePlayerInfo> result = new List<MtgMeleePlayerInfo>();

            string pageContent = new WebClient().DownloadString(uri);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);

            var phaseNode = doc.DocumentNode.SelectNodes("//div[@id='standings-phase-selector-container']").First();
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
                        Standing = standing,
                        DeckUris = playerDeckListIds.Select(id => new Uri(MtgMeleeConstants.DeckPage.Replace("{deckId}", id))).ToArray()
                    });
                }

                offset += 25;
            } while (hasData && (!maxPlayers.HasValue || offset < maxPlayers.Value));

            return result.ToArray();
        }
        private static string NormalizeSpaces(string data)
        {
            return Regex.Replace(data, "\\s+", " ").Trim();
        }
    }
}
