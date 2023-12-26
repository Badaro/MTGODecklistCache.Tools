using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Tools;

namespace MTGODecklistCache.Normalizer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: MTGODecklistCache.Normalizer.App CACHE_FOLDER");
                return;
            }

            Console.WriteLine("Starting normalization");
            int count = 0;
            string[] tournamentFiles = Directory.GetFiles(args[0], "*.json", SearchOption.AllDirectories);
            Console.Write($"Normalizing tournaments: {count}/{tournamentFiles.Length}");

            HashSet<string> tournamentsWithValidationErrors = new HashSet<string>();
            List<string> validationErrors = new List<string>();
            Dictionary<string, List<string>> cardValidationErrors = new Dictionary<string, List<string>>();

            foreach (string tournamentFile in tournamentFiles)
            {
                if (count % 100 == 0) Console.Write($"\rNormalizing tournaments: {count}/{tournamentFiles.Length}"); ;
                count++;

                CacheItem tournament = JsonConvert.DeserializeObject<CacheItem>(File.ReadAllText(tournamentFile));
                tournament.Decks = tournament.Decks.Select(d => DeckNormalizer.Normalize(d)).ToArray();
                File.WriteAllText(tournamentFile, JsonConvert.SerializeObject(tournament, Formatting.Indented));
            }

            Console.WriteLine($"\rNormalizing tournaments: {tournamentFiles.Length}/{tournamentFiles.Length}"); ;
            Console.WriteLine("Finished tournament normalization");
        }
    }
}