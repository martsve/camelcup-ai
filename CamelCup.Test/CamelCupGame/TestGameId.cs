using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;
using System.Collections.Generic;
using System;

namespace CamelCup.Test
{
    [TestClass]
    public class TestGameId
    {
        [TestMethod]
        public void CamelCupGame_GameId_BaseCase()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var players = new List<Player> () {
                GetTestBot(1),
                GetTestBot(2),
                GetTestBot(3),
                GetTestBot(4),
            };
            var game = new CamelCupGame(players, startingPositions, 1);
            
            Assert.AreEqual("1cad8d5c-be09-8a0b-a3ab-2a828eb02aab", game.GameId.ToString());
        }

        [TestMethod]
        public void CamelCupGame_GameId_ChangePlayerOrder()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var players = new List<Player> () {
                GetTestBot(2),
                GetTestBot(1),
                GetTestBot(3),
                GetTestBot(4),
            };
            var game = new CamelCupGame(players, startingPositions, 1);
            
            Assert.AreEqual("1c659be2-875a-c107-676b-6dd92d020507", game.GameId.ToString());
        }

        [TestMethod]
        public void CamelCupGame_GameId_ChangeStartingPosition()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("2,0 1,1 2,2 3,3 3,4");
            var players = new List<Player> () {
                GetTestBot(1),
                GetTestBot(2),
                GetTestBot(3),
                GetTestBot(4),
            };
            var game = new CamelCupGame(players, startingPositions, 1);
            
            Assert.AreEqual("9a41598a-615f-ebe3-f5ac-fcd885929f9e", game.GameId.ToString());
        }

            [TestMethod]
        public void CamelCupGame_GameId_ChangeSeed()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var players = new List<Player> () {
                GetTestBot(1),
                GetTestBot(2),
                GetTestBot(3),
                GetTestBot(4),
            };
            var game = new CamelCupGame(players, startingPositions, 2);
            
            Assert.AreEqual("ac6a60b6-3666-09ab-cc0e-51c2c15915b0", game.GameId.ToString());
        }

        private Player GetTestBot(int id)
        {
            var bot = new RandomBot(id);
            var player = new Player(bot, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            player.Name = bot.GetPlayerName();
            return player;
        }
    }
}
