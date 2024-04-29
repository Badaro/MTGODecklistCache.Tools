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
        MtgMeleePlayerInfo[] _playersRequiringMatch;
        MtgMeleePlayerInfo[] _playersOnlyFirstPage;
        MtgMeleePlayerInfo[] _playersMissing;

        [OneTimeSetUp]
        public void LoadPlayers()
        {
            _players = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/72980"));
            _playersRequiringMatch = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/16429"));
            _playersOnlyFirstPage = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/16429"), 25);
            _playersMissing = new MtgMeleeClient().GetPlayers(new Uri("https://melee.gg/Tournament/View/31590"));
        }

        [Test]
        public void ShouldLoadNumberOfPlayers()
        {
            _players.Count().Should().Be(207);
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
            _players.First().Result.Should().Be("18-1-0");
        }

        [Test]
        public void ShouldCorrectlyMapPlayerNameToUserName()
        {
            _playersRequiringMatch.First(p => p.PlayerName == "koki hara").UserName.Should().Be("BlooMooNight");
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
                Player = "Yoshihiko Ikawa",
                Rank = 1,
                Points = 54,
                OMWP = 0.62502867,
                GWP = 0.75,
                OGWP = 0.57640079,
                Wins = 18,
                Losses = 1,
                Draws = 0
            });
        }

        [Test]
        public void ShouldIncludeCorrectStandingsDataWithDraws()
        {
            _players.Skip(1).First().Standing.Should().BeEquivalentTo(new Standing()
            {
                Player = "Yuta Takahashi",
                Rank = 2,
                Points = 41,
                OMWP = 0.59543296,
                GWP = 0.68027211,
                OGWP = 0.55188929,
                Wins = 13,
                Losses = 4,
                Draws = 2
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
            _players.Skip(7).First().DeckUris.Should().BeEquivalentTo(new Uri[]
            {
                new Uri("https://melee.gg/Decklist/View/391605")
            });
        }

        [Test]
        public void ShouldLoadCorrectDeckUrisWhenMultiplePresent()
        {
            _players.First().DeckUris.Should().BeEquivalentTo(new Uri[]
            {
                new Uri("https://melee.gg/Decklist/View/391788"),
                new Uri("https://melee.gg/Decklist/View/393380")
            });
        }
    }
}