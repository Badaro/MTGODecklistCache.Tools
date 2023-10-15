using HtmlAgilityPack;
using MTGODecklistCache.Updater.MtgMelee.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace MTGODecklistCache.Updater.MtgMelee
{
    public static class TournamentInfoLoader
    {
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

            var phaseNode = doc.DocumentNode.SelectNodes("//div[@id='standings-phase-selector-container']");
            result.HasDecklists = phaseNode != null;

            return result;
        }
    }
}
