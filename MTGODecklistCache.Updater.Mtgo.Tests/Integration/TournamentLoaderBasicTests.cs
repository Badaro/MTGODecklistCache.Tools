using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class TournamentLoaderBasicTests
    {
        private Tournament[] _testData = null;

        [OneTimeSetUp]
        public void GetTestData()
        {
            _testData = new MtgoSource().GetTournaments(new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc));
        }

        [Test]
        public void TournamentCountIsCorrect()
        {
            _testData.Length.Should().Be(15);
        }

        [Test]
        public void TournamentDataIsCorrect()
        {
            _testData.Should().BeEquivalentTo(new Tournament[]
            {
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Limited Super Qualifier", Uri=new Uri("https://www.mtgo.com/decklist/limited-super-qualifier-2020-06-0212162899"), JsonFile="limited-super-qualifier-2020-06-0212162899.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Limited Super Qualifier", Uri=new Uri("https://www.mtgo.com/decklist/limited-super-qualifier-2020-06-0212162898"), JsonFile="limited-super-qualifier-2020-06-0212162898.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2020-06-025082"),               JsonFile="modern-league-2020-06-025082.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2020-06-025090"),               JsonFile="pauper-league-2020-06-025090.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2020-06-025098"),              JsonFile="pioneer-league-2020-06-025098.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2020-06-025106"),             JsonFile="standard-league-2020-06-025106.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2020-06-025182"),              JsonFile="vintage-league-2020-06-025182.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 02, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2020-06-025190"),               JsonFile="legacy-league-2020-06-025190.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern Super Qualifier",  Uri=new Uri("https://www.mtgo.com/decklist/modern-super-qualifier-2020-06-0112162897"),  JsonFile="modern-super-qualifier-2020-06-0112162897.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Modern League",           Uri=new Uri("https://www.mtgo.com/decklist/modern-league-2020-06-015082"),               JsonFile="modern-league-2020-06-015082.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pauper League",           Uri=new Uri("https://www.mtgo.com/decklist/pauper-league-2020-06-015090"),               JsonFile="pauper-league-2020-06-015090.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Pioneer League",          Uri=new Uri("https://www.mtgo.com/decklist/pioneer-league-2020-06-015098"),              JsonFile="pioneer-league-2020-06-015098.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Standard League",         Uri=new Uri("https://www.mtgo.com/decklist/standard-league-2020-06-015106"),             JsonFile="standard-league-2020-06-015106.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Vintage League",          Uri=new Uri("https://www.mtgo.com/decklist/vintage-league-2020-06-015182"),              JsonFile="vintage-league-2020-06-015182.json" },
                new Tournament(){ Date=new DateTime(2020, 06, 01, 00, 00, 00, DateTimeKind.Utc), Name="Legacy League",           Uri=new Uri("https://www.mtgo.com/decklist/legacy-league-2020-06-015190"),               JsonFile="legacy-league-2020-06-015190.json" },
            });
        }
    }
}