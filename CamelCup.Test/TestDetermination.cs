using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;

namespace CamelCup.Test
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
            var game = runner.ComputeNewGame(steps: 1);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;
            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");

            Assert.AreEqual(3, gameState.Money[0], "player 0 money");
            Assert.AreEqual(3, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_5()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeNewGame(steps: 5);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");

            Assert.AreEqual(5, gameState.Money[0], "player 0 money");
            Assert.AreEqual(3, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_15()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeNewGame(steps: 15);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(1, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");

            Assert.AreEqual(8, gameState.Money[0], "player 0 money");
            Assert.AreEqual(6, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_50()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeNewGame(steps: 50);    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");

            Assert.AreEqual(8, gameState.Money[0], "player 0 money");
            Assert.AreEqual(17, gameState.Money[1], "player 1 money");

            Assert.AreEqual(0, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");
        }

        [TestMethod]
        public void Integration_CamelRunner_Valid()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeNewGame();    
            var players = runner.GetPlayers().ToList();

            var gameState = game.GameState;
            Assert.AreEqual(16, gameState.BoardSize, "board size");
            Assert.AreEqual(5, gameState.Camels.Count, "camel count");
            
            Assert.AreEqual(82, runner.Step, "game step");

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");

            Assert.AreEqual(32, gameState.Money[0], "player 0 money");
            Assert.AreEqual(37, gameState.Money[1], "player 1 money");

            Assert.AreEqual(0, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");
        }
    }
}
