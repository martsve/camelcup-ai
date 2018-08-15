using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup.MartinBots;
using System.Linq;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestDeterminationSteps
    {
        [TestMethod]
        public void Integration_CamelRunner_Step_1()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 1);    
            var players = runner.GetPlayers();

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
            var players = runner.GetPlayers();

            var gameState = game.GameState;

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");
            Assert.AreEqual(0, gameState.Round, "round");

            Assert.AreEqual(4, gameState.Money[0], "player 0 money");
            Assert.AreEqual(5, gameState.Money[1], "player 1 money");

            Assert.AreEqual(0, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");

            Assert.AreEqual("0,0 4,3 4,4 1,1 2,2", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_15()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 15);    
            var players = runner.GetPlayers();

            var gameState = game.GameState;

            Assert.AreEqual(1, game.StartingPlayer, "starting player");
            Assert.AreEqual(1, game.CurrentPlayer, "current player");
            Assert.AreEqual(1, gameState.Round, "round");

            Assert.AreEqual(10, gameState.Money[0], "player 0 money");
            Assert.AreEqual(8, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");

            Assert.AreEqual("4,2 4,1 8,4 4,3 2,0", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Step_50()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3, steps: 50);    
            var players = runner.GetPlayers();

            var gameState = game.GameState;

            Assert.AreEqual(1, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");
            Assert.AreEqual(3, gameState.Round, "round");

            Assert.AreEqual(19, gameState.Money[0], "player 0 money");
            Assert.AreEqual(14, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");

            Assert.AreEqual("10,3 10,1 10,2 10,4 6,0", gameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_CamelRunner_Valid()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeSeededGame(1, 2, 3);    
            var players = runner.GetPlayers();

            var gameState = game.GameState;
            
            Assert.AreEqual(87, runner.Step, "game step");

            Assert.AreEqual(1, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");
            Assert.AreEqual(5, gameState.Round, "round");

            Assert.AreEqual(29, gameState.Money[0], "player 0 money");
            Assert.AreEqual(24, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(0, players[1].Wins, "player 1 wins");

            Assert.AreEqual("16,4 12,0 13,1 16,3 13,2", gameState.CamelPositionToString(), "camel positions");
        }
    }
}
