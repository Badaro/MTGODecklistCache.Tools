using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using MTGODecklistCache.Updater.MtgMelee;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MTGODecklistCache.Updater.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: MTGODecklistCache.Updater.App CACHE_FOLDER RAWDATA_FOLDER [START_DATE] [END_DATE]");
                return;
            }

            string cacheFolder = new DirectoryInfo(args[0]).FullName;
            string rawDataFolder = new DirectoryInfo(args[1]).FullName;

            DateTime startDate = DateTime.Now.AddDays(-14).ToUniversalTime().Date;
            if (args.Length > 2)
            {
                startDate = DateTime.Parse(args[2], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }
            DateTime? endDate = null;
            if (args.Length > 3)
            {
                endDate = DateTime.Parse(args[3], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }

            UpdateFolder(cacheFolder, new Mtgo.MtgoSource(), startDate, endDate);
            UpdateFolder(cacheFolder, new ManaTraders.ManaTradersSource(), startDate, endDate);
            UpdateFolder(cacheFolder, new MtgMelee.MtgMeleeSource(rawDataFolder), startDate, endDate);
        }

        static void UpdateFolder<T>(string cacheRootFolder, ITournamentSource<T> source, DateTime startDate, DateTime? endDate) 
            where T : Tournament 
        {
            string cacheFolder = Path.Combine(cacheRootFolder, source.Provider);

            Console.WriteLine($"Downloading tournament list for {source.Provider}");
            foreach (var tournament in source.GetTournaments(startDate, endDate).OrderBy(t => t.Date))
            {
                string targetFolder = Path.Combine(cacheFolder, tournament.Date.Year.ToString(), tournament.Date.Month.ToString("D2").ToString(), tournament.Date.Day.ToString("D2").ToString());
                if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                string targetFile = Path.Combine(targetFolder, tournament.JsonFile);
                if (File.Exists(targetFile))
                {
                    continue;
                }

                Console.WriteLine($"- Downloading tournament {tournament.JsonFile}");
                var details = source.GetTournamentDetails(tournament);
                if(details.Decks==null)
                {
                    Console.WriteLine($"-- Tournament has no decks, skipping");
                    continue;
                }

                string contents = JsonConvert.SerializeObject(details, Formatting.Indented);

                File.WriteAllText(targetFile, contents);
            }
        }
    }
}
