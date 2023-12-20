using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class TournamentLoaderCrossMonthTests
    {
        private Tournament[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgoSource().GetTournaments(new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            _testData.Length.Should().Be(21);
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            _testData.Should().BeEquivalentTo(new Tournament[]
            {
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern Preliminary",      Uri=new Uri("https://www.mtgo.com/decklist/modern-preliminary-2020-06-0112162949"),      JsonFile="modern-preliminary-2020-06-0112162949.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern Super Qualifier",  Uri=new Uri("https://www.mtgo.com/decklist/modern-super-qualifier-2020-06-0112162897"),  JsonFile="modern-super-qualifier-2020-06-0112162897.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2020-06-015082"),               JsonFile="modern-league-2020-06-015082.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2020-06-015090"),               JsonFile="pauper-league-2020-06-015090.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2020-06-015098"),              JsonFile="pioneer-league-2020-06-015098.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2020-06-015106"),             JsonFile="standard-league-2020-06-015106.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2020-06-015182"),              JsonFile="vintage-league-2020-06-015182.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2020-06-015190"),               JsonFile="legacy-league-2020-06-015190.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Vintage Preliminary",     Uri=new Uri("https://www.mtgo.com/decklist/vintage-preliminary-2020-05-3112162942"),     JsonFile="vintage-preliminary-2020-05-3112162942.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Standard Challenge",      Uri=new Uri("https://www.mtgo.com/decklist/standard-challenge-2020-05-3112162941"),      JsonFile="standard-challenge-2020-05-3112162941.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Pauper Challenge",        Uri=new Uri("https://www.mtgo.com/decklist/pauper-challenge-2020-05-3112162939"),        JsonFile="pauper-challenge-2020-05-3112162939.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Legacy Challenge",        Uri=new Uri("https://www.mtgo.com/decklist/legacy-challenge-2020-05-3112162938"),        JsonFile="legacy-challenge-2020-05-3112162938.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Modern Challenge",        Uri=new Uri("https://www.mtgo.com/decklist/modern-challenge-2020-05-3112162936"),        JsonFile="modern-challenge-2020-05-3112162936.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Limited Super Qualifier", Uri=new Uri("https://www.mtgo.com/decklist/limited-super-qualifier-2020-05-3112162896"), JsonFile="limited-super-qualifier-2020-05-3112162896.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Vintage Challenge",       Uri=new Uri("https://www.mtgo.com/decklist/vintage-challenge-2020-05-3112162935"),       JsonFile="vintage-challenge-2020-05-3112162935.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2020-05-315082"),               JsonFile="modern-league-2020-05-315082.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2020-05-315090"),               JsonFile="pauper-league-2020-05-315090.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2020-05-315098"),              JsonFile="pioneer-league-2020-05-315098.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2020-05-315106"),             JsonFile="standard-league-2020-05-315106.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2020-05-315182"),              JsonFile="vintage-league-2020-05-315182.json" },
                new Tournament(){ Date=new DateTime(2020, 05, 31, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2020-05-315190"),               JsonFile="legacy-league-2020-05-315190.json" },
            });
        }
    }
}