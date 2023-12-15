using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class LotrNameErrorTests_REVIEW
    {
        [Test]
        public void ShouldFixNameForLorienRevealed()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/pauper-league-2023-06-21")
            }).Decks
                .First(d => d.Player == "Selkcahs")
                .Mainboard
                .First(c => c.CardName.EndsWith("Revealed")).CardName
                .Should().Be("L�rien Revealed");
        }

        [Test]
        public void ShouldFixNameForTrollofKhazaddum()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/modern-preliminary-2023-06-2212560103")
            }).Decks
                .First(d => d.Player == "Mighty_Ravendark")
                .Mainboard
                .First(c => c.CardName.StartsWith("Troll")).CardName
                .Should().Be("Troll of Khazad-d�m");
        }

        [Test]
        public void ShouldFixNameForPalantirofOrthanc()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/modern-league-2023-06-23")
            }).Decks
                .First(d => d.Player == "Craigthefirst")
                .Mainboard
                .First(c => c.CardName.StartsWith("Palan")).CardName
                .Should().Be("Palant�r of Orthanc");
        }

        [Test]
        public void ShouldFixNameForSmeagolHelpfulGuide()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/legacy-challenge-32-2023-07-0812564210")
            }).Decks
                .First(d => d.Player == "Koike")
                .Mainboard
                .First(c => c.CardName.EndsWith("Helpful Guide")).CardName
                .Should().Be("Sm�agol, Helpful Guide");
        }

        [Test]
        public void ShouldFixNameForBaraddur()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/modern-league-2023-07-11")
            }).Decks
                .First(d => d.Player == "julianMTG")
                .Mainboard
                .First(c => c.CardName.StartsWith("Barad")).CardName
                .Should().Be("Barad-d�r");
        }

        [Test]
        public void ShouldFixNameForAndurilFlameOfTheWest()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/legacy-league-2023-07-22")
            }).Decks
                .First(d => d.Player == "EvanW")
                .Mainboard
                .First(c => c.CardName.EndsWith("Flame of the West")).CardName
                .Should().Be("And�ril, Flame of the West");
        }

        [Test]
        public void ShouldFixNameForGrimaWormtongue()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "straca3")
                .Mainboard
                .First(c => c.CardName.EndsWith("Wormtongue")).CardName
                .Should().Be("Gr�ma Wormtongue");
        }

        [Test]
        public void ShouldFixNameForGrishnakhBrashInstigator()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "AliasV")
                .Mainboard
                .First(c => c.CardName.EndsWith("Brash Instigator")).CardName
                .Should().Be("Grishn�kh, Brash Instigator");
        }

        [Test]
        public void ShouldFixNameForEomerMarshalofRohan()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "perret")
                .Mainboard
                .First(c => c.CardName.EndsWith("Marshal of Rohan")).CardName
                .Should().Be("�omer, Marshal of Rohan");
        }

        [Test]
        public void ShouldFixNameForEomeroftheRiddermark()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "perret")
                .Mainboard
                .First(c => c.CardName.EndsWith("of the Riddermark")).CardName
                .Should().Be("�omer of the Riddermark");
        }

        [Test]
        public void ShouldFixNameForDunedainBlade()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "perret")
                .Sideboard
                .First(c => c.CardName.EndsWith("dain Blade")).CardName
                .Should().Be("D�nedain Blade");
        }

        [Test]
        public void ShouldFixNameForNazgul()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "straca3")
                .Mainboard
                .First(c => c.CardName.StartsWith("Naz")).CardName
                .Should().Be("Nazg�l");
        }

        [Test]
        public void ShouldFixNameForLothlorienLookout()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "_Holzi_")
                .Mainboard
                .First(c => c.CardName.EndsWith("Lookout")).CardName
                .Should().Be("Lothl�rien Lookout");
        }

        [Test]
        public void ShouldFixNameForGaladrielofLothlorien()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "_Holzi_")
                .Mainboard
                .First(c => c.CardName.StartsWith("Galadriel of")).CardName
                .Should().Be("Galadriel of Lothl�rien");
        }

        [Test]
        public void ShouldFixNameForArwenUndomiel()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "_Holzi_")
                .Mainboard
                .First(c => c.CardName.StartsWith("Arwen Un")).CardName
                .Should().Be("Arwen Und�miel");
        }

        [Test]
        public void ShouldFixNameForDunedainRangers()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "_Holzi_")
                .Sideboard
                .First(c => c.CardName.EndsWith("dain Rangers")).CardName
                .Should().Be("D�nedain Rangers");
        }

        [Test]
        public void ShouldFixNameForMauhurUrukhaiCaptain()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "Fappi")
                .Mainboard
                .First(c => c.CardName.EndsWith("Uruk-hai Captain")).CardName
                .Should().Be("Mauh�r, Uruk-hai Captain");
        }

        [Test]
        public void ShouldFixNameForSoothingofSmeagol()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2212568041")
            }).Decks
                .First(d => d.Player == "AliasV")
                .Mainboard
                .First(c => c.CardName.StartsWith("Soothing of")).CardName
                .Should().Be("Soothing of Sm�agol");
        }

        [Test]
        public void ShouldFixNameForTheodenKingofRohan()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2312568042")
            }).Decks
                .First(d => d.Player == "stayrospet")
                .Mainboard
                .First(c => c.CardName.EndsWith("oden, King of Rohan")).CardName
                .Should().Be("Th�oden, King of Rohan");
        }

        [Test]
        public void ShouldFixNameForTaleofTinuviel()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2312568042")
            }).Decks
                .First(d => d.Player == "stayrospet")
                .Sideboard
                .First(c => c.CardName.StartsWith("Tale of")).CardName
                .Should().Be("Tale of Tin�viel");
        }

        [Test]
        public void ShouldFixNameForEowynLadyofRohan()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2312568042")
            }).Decks
                .First(d => d.Player == "El_Corni")
                .Mainboard
                .First(c => c.CardName.EndsWith("Lady of Rohan")).CardName
                .Should().Be("�owyn, Lady of Rohan");
        }

        [Test]
        public void ShouldFixNameForUglukoftheWhiteHand()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/mocs-showcase-open-2023-07-2312568042")
            }).Decks
                .First(d => d.Player == "Ishan_")
                .Sideboard
                .First(c => c.CardName.EndsWith("of the White Hand")).CardName
                .Should().Be("Ugl�k of the White Hand");
        }

        [Test]
        public void ShouldFixNameForEowynFearlessKnight()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/en/mtgo/decklist/limited-super-qualifier-2023-08-0612571765")
            }).Decks
                .First(d => d.Player == "L1X0")
                .Mainboard
                .First(c => c.CardName.EndsWith("Fearless Knight")).CardName
                .Should().Be("�owyn, Fearless Knight");
        }
    }
}