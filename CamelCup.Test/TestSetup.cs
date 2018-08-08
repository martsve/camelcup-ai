using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;

namespace CamelCup.Test
{
    [TestClass]
    public class TestSetup
    {
        [TestMethod]
        public void Integration_CamelRunner_Step_0()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = runner.ComputeNewGame(steps: 0);    
            var players = runner.GetPlayers().ToList();
            var gameState = game.GameState;
            var camelOrder = gameState.GetLeadingOrder();

            Assert.AreEqual(2, runner.GetPlayers().Count(), "players");
            Assert.AreEqual(16, gameState.BoardSize, "board");
            Assert.AreEqual(5, gameState.Camels.Count, "camels");

            Assert.AreEqual(CamelColor.Yellow, camelOrder.Skip(0).First(), "camel 0");
            Assert.AreEqual(CamelColor.Orange, camelOrder.Skip(1).First(), "camel 1");
            Assert.AreEqual(CamelColor.Green, camelOrder.Skip(2).First(), "camel 2");
            Assert.AreEqual(CamelColor.Blue, camelOrder.Skip(3).First(), "camel 3");
            Assert.AreEqual(CamelColor.Red, camelOrder.Skip(4).First(), "camel 4");

            Assert.AreEqual(0, game.StartingPlayer, "starting player");
            Assert.AreEqual(0, game.CurrentPlayer, "current player");

            Assert.AreEqual(3, gameState.Money[0], "player 0 money");
            Assert.AreEqual(3, gameState.Money[1], "player 1 money");

            Assert.AreEqual(1, players[0].Wins, "player 0 wins");
            Assert.AreEqual(1, players[1].Wins, "player 1 wins");
        }
    }
}
