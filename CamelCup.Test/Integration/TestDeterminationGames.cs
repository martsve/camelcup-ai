using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup.MartinBots;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestDeterminationGames
    {
        [TestMethod]
        public void Integration_Determination_1()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());

            for (int i = 0; i < 2; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 1159066037, playerOrderSeed: 1561971233, gameSeed: 778305510);    

                Assert.AreEqual("97089d38-319b-5841-5671-da55998a82d2", game.GameId.ToString(), "game id");
                Assert.AreEqual("34 3 17", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("18,4 11,0 11,1 18,3 18,2", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_2()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            for (int i = 0; i < 2; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 648398875, playerOrderSeed: 1276980857, gameSeed: 701934574);    

                Assert.AreEqual("f5ec7765-86da-5cae-15d2-4f7add46ea6f", game.GameId.ToString(), "game id");
                Assert.AreEqual("4 19 31 40", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("9,1 14,2 9,0 16,4 15,3", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_3()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            for (int i = 0; i < 2; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 1756550859, playerOrderSeed: 1594470206, gameSeed: 825977671);    

                Assert.AreEqual("a4142202-c7b4-de85-7f63-e71282ec28dc", game.GameId.ToString(), "game id");
                Assert.AreEqual("38 0 19 4", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("10,0 15,3 10,1 13,2 16,4", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_4()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            for (int i = 0; i < 2; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 462229339, playerOrderSeed: 1329827054, gameSeed: 1471678946);    

                Assert.AreEqual("41 0 21 29", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("10,1 17,4 13,3 13,2 10,0", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        
        [TestMethod]
        public void Integration_Determination_5()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new SmartMartinPlayer());
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());

            for (int i = 0; i < 2; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 1989079677, playerOrderSeed: 1448447844, gameSeed: 2101643186);    

                Assert.AreEqual("33 18 15 23", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("17,4 14,1 11,0 14,3 14,2", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_6()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new RandomBot());

            for (int i = 0; i < 3; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 169572456, playerOrderSeed: 1271470828, gameSeed: 677344959);    

                Assert.AreEqual("11 23 20", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("13,0 16,2 16,4 13,1 16,3", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_7()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new RandomBot());

            for (int i = 0; i < 3; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 997538913, playerOrderSeed: 1893693153, gameSeed: 1620525773);    

                Assert.AreEqual("19 23 25", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("12,2 11,1 15,3 6,0 16,4", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_8()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new IllegalBot());

            for (int i = 0; i < 3; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 713137502, playerOrderSeed: 791917098, gameSeed: 1673969849);    

                Assert.AreEqual("0 0 25 0 0", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("8,0 13,4 11,2 11,1 11,3", game.GameState.CamelPositionToString(), "camel positions");
            }
        }

        [TestMethod]
        public void Integration_Determination_9()
        {
            var runner = new CamelRunner();
            runner.AddPlayer(new RandomBot());
            runner.AddPlayer(new RandomBot());

            for (int i = 0; i < 3; i++) {
                var game = runner.ComputeSeededGame(startPosSeed: 1412629303, playerOrderSeed: 1986369960, gameSeed: 1248783280);    

                Assert.AreEqual("30 44", game.GameState.MoneyToString(), "player money");
                Assert.AreEqual("5,0 14,3 12,1 16,4 14,2", game.GameState.CamelPositionToString(), "camel positions");
            }
        }
    }
}

