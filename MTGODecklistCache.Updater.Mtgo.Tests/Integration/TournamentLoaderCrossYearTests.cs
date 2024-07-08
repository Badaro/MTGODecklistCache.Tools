using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class TournamentLoaderCrossYearTests
    {
        private Tournament[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgoSource().GetTournaments(new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            _testData.Length.Should().Be(18);
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            _testData.Should().BeEquivalentTo(new Tournament[]
            {
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer Challenge",       Uri=new Uri("https://www.mtgo.com/decklist/pioneer-challenge-2022-01-0112367816"),       JsonFile="pioneer-challenge-2022-01-0112367816.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Vintage Challenge",       Uri=new Uri("https://www.mtgo.com/decklist/vintage-challenge-2022-01-0112367814"),       JsonFile="vintage-challenge-2022-01-0112367814.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern Challenge",        Uri=new Uri("https://www.mtgo.com/decklist/modern-challenge-2022-01-0112367813"),        JsonFile="modern-challenge-2022-01-0112367813.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Convention Championship", Uri=new Uri("https://www.mtgo.com/decklist/convention-championship-2022-01-0112367722"), JsonFile="convention-championship-2022-01-0112367722.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Standard Challenge",      Uri=new Uri("https://www.mtgo.com/decklist/standard-challenge-2022-01-0112367812"),      JsonFile="standard-challenge-2022-01-0112367812.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pauper Challenge",        Uri=new Uri("https://www.mtgo.com/decklist/pauper-challenge-2022-01-0112367810"),        JsonFile="pauper-challenge-2022-01-0112367810.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2022-01-016237"),               JsonFile="legacy-league-2022-01-016237.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2022-01-016245"),               JsonFile="modern-league-2022-01-016245.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2022-01-016253"),               JsonFile="pauper-league-2022-01-016253.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2022-01-016261"),              JsonFile="pioneer-league-2022-01-016261.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2022-01-016269"),             JsonFile="standard-league-2022-01-016269.json" },
                new Tournament(){ Date=new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2022-01-016277"),              JsonFile="vintage-league-2022-01-016277.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2021-12-316237"),               JsonFile="legacy-league-2021-12-316237.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2021-12-316245"),               JsonFile="modern-league-2021-12-316245.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2021-12-316253"),               JsonFile="pauper-league-2021-12-316253.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2021-12-316261"),              JsonFile="pioneer-league-2021-12-316261.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2021-12-316269"),             JsonFile="standard-league-2021-12-316269.json" },
                new Tournament(){ Date=new DateTime(2021, 12, 31, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2021-12-316277"),              JsonFile="vintage-league-2021-12-316277.json" },
            });
        }
    }
}