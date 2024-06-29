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

namespace MTGODecklistCache.Updater.Topdeck
{
    public class TopdeckSource : ITournamentSource<Tournament>
    {
        public string Provider => throw new NotImplementedException();

        public CacheItem GetTournamentDetails(Tournament tournament)
        {
            TopdeckClient client = new TopdeckClient();

            //var tournamentData = client.GetTournaments(new Client.Model.TopdeckTournamentRequest()
            //{
            //    Start = new DateTimeOffset(tournament.Date.Date,TimeSpan.Zero).ToUnixTimeSeconds(),
            //    End= new DateTimeOffset(tournament.Date.Date.AddDays(1), TimeSpan.Zero).ToUnixTimeSeconds(),
            //     Game = Game.MagicTheGathering,
            //     Format = Format.
            //});

            throw new NotImplementedException();
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
                    var tournaments = new TopdeckClient().GetTournaments(new()
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
                            JsonFile = FilenameGenerator.GenerateFileName(tournament.ID, tournament.Name, date, format.ToString(), validFormats.Select(f => f.ToString()).ToArray(), 0)
                        });
                    }
                }

                startDate = startDate.AddDays(7);
            }
            Console.WriteLine($"\r[Topddeck] Download finished".PadRight(LogSettings.BufferWidth));

            return result.ToArray();
        }
    }
}
