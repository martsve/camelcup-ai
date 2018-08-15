using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup.MartinBots;
using System.Linq;
using System;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestDetermination2
    {
        [TestMethod]
        public void Integration_Determination_1()
        {
            var runner = new CamelRunner(seed: -639947146);
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());

            var game = runner.ComputeSeededGame(startPosSeed: 1159066037, playerOrderSeed: 1561971233, gameSeed: 778305510);    

            Assert.AreEqual("97089d38-319b-5841-5671-da55998a82d2", game.GameId.ToString(), "game id");
            Assert.AreEqual("34 3 17", game.GameState.MoneyToString(), "player money");
            Assert.AreEqual("18,4 11,0 11,1 18,3 18,2", game.GameState.CamelPositionToString(), "camel positions");

        }

        [TestMethod]
        public void Integration_Determination_2()
        {
            var runner = new CamelRunner(seed: -1775057943);
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            var game = runner.ComputeSeededGame(startPosSeed: 648398875, playerOrderSeed: 1276980857, gameSeed: 701934574);    

            Assert.AreEqual("f5ec7765-86da-5cae-15d2-4f7add46ea6f", game.GameId.ToString(), "game id");
            Assert.AreEqual("4 19 31 40", game.GameState.MoneyToString(), "player money");
            Assert.AreEqual("9,1 14,2 9,0 16,4 15,3", game.GameState.CamelPositionToString(), "camel positions");
        }

        [TestMethod]
        public void Integration_Determination_3()
        {
            var runner = new CamelRunner(seed: 159138478);
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            var game = runner.ComputeSeededGame(startPosSeed: 1756550859, playerOrderSeed: 1594470206, gameSeed: 825977671);    

            Assert.AreEqual("a4142202-c7b4-de85-7f63-e71282ec28dc", game.GameId.ToString(), "game id");
            Assert.AreEqual("38 0 19 4", game.GameState.MoneyToString(), "player money");
            Assert.AreEqual("10,0 15,3 10,1 13,2 16,4", game.GameState.CamelPositionToString(), "camel positions");
        }
    }
}
