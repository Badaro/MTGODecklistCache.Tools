using HtmlAgilityPack;
using MTGODecklistCache.Updater.MtgMelee.Model;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
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
    }
}
