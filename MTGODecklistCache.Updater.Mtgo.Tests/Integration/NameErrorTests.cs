using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.Mtgo;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace MTGODecklistCache.Updater.Mtgo.Tests
{
    public class NameErrorTests
    {

        [Test]
        public void ShouldFixCasingForAltarOfDementia()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/modern-challenge-2019-07-0611911516")
            }).Decks
                .First(d => d.Player == "Reniro")
                .Mainboard
                .First(c => c.CardName.EndsWith("Dementia")).CardName
                .Should().Be("Altar of Dementia");
        }

        [Test]
        public void ShouldFixCasingForRainOfTears()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/modern-preliminary-2023-07-1112564240")
            }).Decks
                .First(d => d.Player == "NicolasGEM")
                .Sideboard
                .First(c => c.CardName.EndsWith("Tears")).CardName
                .Should().Be("Rain of Tears");
        }

        [Test]
        public void ShouldFixCasingForSolKanartheTainted()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/standard-challenge-2022-09-0312468578")
            }).Decks
                .First(d => d.Player == "ruin000")
                .Mainboard
                .First(c => c.CardName.EndsWith("Tainted")).CardName
                .Should().Be("Sol'Kanar the Tainted");
        }

        [Test]
        public void ShouldFixCasingForFurnaceOfRath()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/power-nine-challenge-2016-02-279434043")
            }).Decks
                .First(d => d.Player == "MasterSplinter")
                .Mainboard
                .First(c => c.CardName.StartsWith("Furnace")).CardName
                .Should().Be("Furnace of Rath");
        }

        [Test]
        public void ShouldFixNameForNameStickerGoblin()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/legacy-preliminary-2023-09-2212582244")
            }).Decks
                .First(d => d.Player == "xJCloud")
                .Mainboard
                .First(c => c.CardName.StartsWith("_____")).CardName
                .Should().Be("_____ Goblin");
        }

        [Test]
        public void ShouldFixNameForNameJotunGrunt()
        {
            new MtgoSource().GetTournamentDetails(new Tournament()
            {
                Uri = new Uri("https://www.mtgo.com/decklist/modern-preliminary-2023-03-3012538292")
            }).Decks
                .First(d => d.Player == "SilentOne9653")
                .Sideboard
                .First(c => c.CardName.EndsWith("Grunt")).CardName
                .Should().Be("J�tun Grunt");
        }
    }
}