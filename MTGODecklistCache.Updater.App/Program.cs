﻿using MTGODecklistCache.Updater.Model;
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

            bool useMtgo = args.Length < 4 || args[3].ToLowerInvariant() == "mtgo";
            bool useManatraders = args.Length < 4 || args[3].ToLowerInvariant() == "manatraders";
            bool useMelee = args.Length < 4 || args[3].ToLowerInvariant() == "melee";

            if (useMtgo) UpdateFolder(cacheFolder, new Mtgo.MtgoSource(), startDate, endDate);
            if (useManatraders) UpdateFolder(cacheFolder, new ManaTraders.ManaTradersSource(), startDate, endDate);
            if (useMelee) UpdateFolder(cacheFolder, new MtgMelee.MtgMeleeSource(), startDate, endDate);
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
                if (details.Decks == null)
                {
                    Console.WriteLine($"-- Tournament has no decks, skipping");
                    continue;
                }

                string contents = JsonConvert.SerializeObject(details, Formatting.None);

                File.WriteAllText(targetFile, contents);
            }
        }
    }
}
