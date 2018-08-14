using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestDetermination
    {
        [TestMethod]
        public void Integration_CamelRunner_Step_1()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 1);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;
            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");
            Assert.AreEqual(0, gameState.Round, "round");

            Assert.AreEqual(3, gameState.Money[0], "player 0 money");
            Assert.AreEqual(3, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");

            Assert.AreEqual("0,0 0,1 0,2 1,3 1,4", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_5()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 5);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");
            Assert.AreEqual(0, gameState.Round, "round");

            Assert.AreEqual(5, gameState.Money[0], "player 0 money");
            Assert.AreEqual(3, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");

            Assert.AreEqual("0,0 3,3 3,4 1,1 2,2", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_15()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 15);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(1, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");
            Assert.AreEqual(1, gameState.Round, "round");

            Assert.AreEqual(7, gameState.Money[0], "player 0 money");
            Assert.AreEqual(7, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");

            Assert.AreEqual("3,3 3,2 3,1 3,4 2,0", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_50()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 50);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");
            Assert.AreEqual(2, gameState.Round, "round");

            Assert.AreEqual(12, gameState.Money[0], "player 0 money");
            Assert.AreEqual(12, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");

            Assert.AreEqual("9,3 11,4 6,1 7,2 4,0", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Valid()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;
            
            Assert.AreEqual(84, runner.Step, "game step");

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");
            Assert.AreEqual(4, gameState.Round, "round");

            Assert.AreEqual(21, gameState.Money[0], "player 0 money");
            Assert.AreEqual(28, gameState.Money[1], "player 1 money");

            Assert.AreEqual(0, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");

            Assert.AreEqual("17,1 17,2 13,0 17,3 17,4", gameState.CamelPositionToString(), "camel positions");
        }
    }
}
