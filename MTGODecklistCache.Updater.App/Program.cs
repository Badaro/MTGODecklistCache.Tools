using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Model.Sources;
using MTGODecklistCache.Updater.MtgMelee;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;

namespace MTGODecklistCache.Updater.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: MTGODecklistCache.Updater.App CACHE_FOLDER [START_DATE] [END_DATE] [SOURCE]");
                return;
            }

            string cacheFolder = new DirectoryInfo(args[0]).FullName;

            DateTime startDate = DateTime.Now.AddDays(-7).ToUniversalTime().Date;
            if (args.Length > 1)
            {
                startDate = DateTime.Parse(args[1], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }
            DateTime? endDate = null;
            if (args.Length > 2)
            {
                endDate = DateTime.Parse(args[2], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }

            bool useMtgo = args.Length < 4 || args[3].ToLowerInvariant() == "mtgo" || args[3].ToLowerInvariant() == "all";
            bool useManatraders = args.Length < 4 || args[3].ToLowerInvariant() == "manatraders" || args[3].ToLowerInvariant() == "all"; ;
            bool useMelee = args.Length < 4 || args[3].ToLowerInvariant() == "melee" || args[3].ToLowerInvariant() == "all"; ;
            bool useTopdeck = args.Length < 4 || args[3].ToLowerInvariant() == "topdeck" || args[3].ToLowerInvariant() == "all"; ;

            bool includeLeagues = args.Length < 5 || args[4].ToLowerInvariant() != "skipleagues";

            if (useMtgo) UpdateFolder(cacheFolder, new Mtgo.MtgoSource(includeLeagues), startDate, endDate);
            if (useManatraders) UpdateFolder(cacheFolder, new ManaTraders.ManaTradersSource(), startDate, endDate);
            if (useMelee) UpdateFolder(cacheFolder, new MtgMelee.MtgMeleeSource(), startDate, endDate);
            if (useTopdeck) UpdateFolder(cacheFolder, new Topdeck.TopdeckSource(), startDate, endDate);
        }

        static void UpdateFolder(string cacheRootFolder, ITournamentSource source, DateTime startDate, DateTime? endDate)
        {
            string cacheFolder = Path.Combine(cacheRootFolder, source.Provider);

            Console.WriteLine($"Downloading tournament list for {source.Provider}");
            foreach (var tournament in source.GetTournaments(startDate, endDate).OrderBy(t => t.Date))
            {
                string targetFolder = Path.Combine(cacheFolder, tournament.Date.Year.ToString(), tournament.Date.Month.ToString("D2").ToString(), tournament.Date.Day.ToString("D2").ToString());
                if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                string targetFile = Path.Combine(targetFolder, tournament.JsonFile);
                if (File.Exists(targetFile) && !tournament.ForceRedownload)
                {
                    continue;
                }

                Console.WriteLine($"- Downloading tournament {tournament.JsonFile}");

                var details = RunWithRetry(() => source.GetTournamentDetails(tournament), 3);
                if (details == null)
                {
                    Console.WriteLine($"-- Tournament has no data, skipping");
                    if (Directory.GetFiles(targetFolder).Length == 0) Directory.Delete(targetFolder);
                    continue;
                }
                if (details.Decks == null)
                {
                    Console.WriteLine($"-- Tournament has no decks, skipping");
                    if (Directory.GetFiles(targetFolder).Length == 0) Directory.Delete(targetFolder);
                    continue;
                }
                if (details.Decks.All(d => d.Mainboard.Count() == 0))
                {
                    Console.WriteLine($"-- Tournament has only empty decks, skipping");
                    if (Directory.GetFiles(targetFolder).Length == 0) Directory.Delete(targetFolder);
                    continue;
                }

                string contents = JsonConvert.SerializeObject(details, Formatting.Indented);

                File.WriteAllText(targetFile, contents);
            }
        }

        static T RunWithRetry<T>(Func<T> action, int maxAttempts)
        {
            int retryCount = 1;
            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    if (retryCount < maxAttempts)
                    {
                        Console.WriteLine($"-- Error '{ex.Message.Trim('.')}' during call, retrying ({++retryCount}/{maxAttempts})");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
