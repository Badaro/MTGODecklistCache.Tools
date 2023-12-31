﻿using FluentAssertions;
using MTGODecklistCache.Updater.Model;
using MTGODecklistCache.Updater.MtgMelee.Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGODecklistCache.Updater.MtgMelee.Client.Tests.Integration
{
    public class PlayerLoaderTests
    {
        MtgMeleePlayerInfo[] _players;
        MtgMeleePlayerInfo[] _playersOnlyFirstPage;
        MtgMeleePlayerInfo[] _playersMissing;

        [OneTimeSetUp]
        public void LoadPlayers()
        {
            _players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/16429"));
            _playersOnlyFirstPage = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/16429"), 25);
            _playersMissing = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/31590"));
        }

        [Test]
        public void ShouldLoadNumberOfPlayers()
        {
            _players.Count().Should().Be(266);
        }

        [Test]
        public void ShouldSupportLimitingPlayers()
        {
            _playersOnlyFirstPage.Count().Should().Be(25);
        }

        [Test]
        public void ShouldIncludeUserNames()
        {
            _players.ToList().ForEach(p => p.UserName.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldIncludePlayerNames()
        {
            _players.ToList().ForEach(p => p.PlayerName.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldIncludeResults()
        {
            _players.ToList().ForEach(p => p.Result.Should().NotBeNullOrEmpty());
        }

        [Test]
        public void ShouldIncludeCorrectResultData()
        {
            _players.First().Result.Should().Be("14-2-0");
        }

        [Test]
        public void ShouldCorrectlyMapPlayerNameToUserName()
        {
            _players.First(p => p.PlayerName == "kouki hara").UserName.Should().Be("BlooMooNight");
        }

        [Test]
        public void ShouldLoadStandings()
        {
            _players.ToList().ForEach(p => p.Standing.Should().NotBeNull());
        }

        [Test]
        public void ShouldIncludeCorrectStandingsData()
        {
            _players.First().Standing.Should().BeEquivalentTo(new Standing()
            {
                Player = "Javier Dominguez",
                Rank = 1,
                Points = 42,
                OMWP = 0.6055560,
                GWP = 0.7631580,
                OGWP = 0.5538550,
                Wins = 14,
                Losses = 2,
                Draws = 0
            });
        }

        [Test]
        public void ShouldIncludeCorrectStandingsDataWithDraws()
        {
            _players.Skip(1).First().Standing.Should().BeEquivalentTo(new Standing()
            {
                Player = "Christian Calcano",
                Rank = 2,
                Points = 37,
                OMWP = 0.6731770,
                GWP = 0.6585370,
                OGWP = 0.5901560,
                Wins = 12,
                Losses = 3,
                Draws = 1
            });
        }

        [Test]
        public void ShouldLoadDeckUris()
        {
            _players.ToList().ForEach(p => p.DeckUris.Should().NotBeNull());
        }

        [Test]
        public void ShouldNotBreakOnEmptyTournaments()
        {
            _playersMissing.Should().BeNull();
        }

        [Test]
        public void ShouldLoadCorrectDeckUris()
        {
            _players.First().DeckUris.Should().BeEquivalentTo(new Uri[]
            {
                new Uri("https://melee.gg/Decklist/View/315233")
            });
        }

        [Test]
        public void ShouldLoadCorrectDeckUrisWhenMultiplePresent()
        {
            _players.Skip(1).First().DeckUris.Should().BeEquivalentTo(new Uri[]
            {
                new Uri("https://melee.gg/Decklist/View/315797"),
                new Uri("https://melee.gg/Decklist/View/315212")
            });
        }

        [Test]
        public void ShouldNotBreakOnMissingPhases()
        {
            new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/2710"))
                .Should().BeNull();
        }
    }
}
