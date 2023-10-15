using HtmlAgilityPack;
using MTGODecklistCache.Updater.MtgMelee.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;

namespace MTGODecklistCache.Updater.MtgMelee
{
    public static class TournamentInfoLoader
    {
        static string _phasePage = "https://melee.gg/Tournament/GetPhaseStandings/{phaseId}";
        static string _phaseParameters = "columns%5B0%5D%5Bdata%5D=Rank&columns%5B0%5D%5Bname%5D=Rank&columns%5B0%5D%5Bsearchable%5D=false&columns%5B0%5D%5Borderable%5D=true&columns%5B0%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B0%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B1%5D%5Bdata%5D=Name&columns%5B1%5D%5Bname%5D=Name&columns%5B1%5D%5Bsearchable%5D=true&columns%5B1%5D%5Borderable%5D=true&columns%5B1%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B1%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B2%5D%5Bdata%5D=Decklists&columns%5B2%5D%5Bname%5D=Decklists&columns%5B2%5D%5Bsearchable%5D=false&columns%5B2%5D%5Borderable%5D=false&columns%5B2%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B2%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B3%5D%5Bdata%5D=Record&columns%5B3%5D%5Bname%5D=Record&columns%5B3%5D%5Bsearchable%5D=false&columns%5B3%5D%5Borderable%5D=false&columns%5B3%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B3%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B4%5D%5Bdata%5D=Points&columns%5B4%5D%5Bname%5D=Points&columns%5B4%5D%5Bsearchable%5D=false&columns%5B4%5D%5Borderable%5D=true&columns%5B4%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B4%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B5%5D%5Bdata%5D=Tiebreaker1&columns%5B5%5D%5Bname%5D=Tiebreaker1&columns%5B5%5D%5Bsearchable%5D=false&columns%5B5%5D%5Borderable%5D=true&columns%5B5%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B5%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B6%5D%5Bdata%5D=Tiebreaker2&columns%5B6%5D%5Bname%5D=Tiebreaker2&columns%5B6%5D%5Bsearchable%5D=false&columns%5B6%5D%5Borderable%5D=true&columns%5B6%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B6%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B7%5D%5Bdata%5D=Tiebreaker3&columns%5B7%5D%5Bname%5D=Tiebreaker3&columns%5B7%5D%5Bsearchable%5D=false&columns%5B7%5D%5Borderable%5D=true&columns%5B7%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B7%5D%5Bsearch%5D%5Bregex%5D=false&order%5B0%5D%5Bcolumn%5D=0&order%5B0%5D%5Bdir%5D=asc&start={start}&length=25&search%5Bvalue%5D=&search%5Bregex%5D=false";

        public static MtgMeleeTournament GetTournamentInfo(Uri uri)
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

            if(phaseNode!=null)
            {
                var phaseId = phaseNode.SelectNodes("button[@class='btn btn-primary round-selector']").Last().Attributes["data-id"].Value;

                string phaseParameters = _phaseParameters.Replace("{start}", "0");
                string phaseUrl = _phasePage.Replace("{phaseId}", phaseId);

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
    }
}
