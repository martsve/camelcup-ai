using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestScoreGame
    {
        [TestMethod]
        public void RulesEngine_ScoreGame_NoBets()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreGame();

            Assert.AreEqual(3, gamestate.Money[0], "money p0");
            Assert.AreEqual(3, gamestate.Money[1], "money p1");
            Assert.AreEqual(3, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }

        [TestMethod]
        public void RulesEngine_ScoreGame_Winners()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(8, startingPositions);

            gamestate.WinnerBets.Add(new GameEndBet() { Player = 7, CamelColor = CamelColor.Yellow }); // 8
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 5, CamelColor = CamelColor.Yellow }); // 5
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 3, CamelColor = CamelColor.Yellow }); // 3
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 2, CamelColor = CamelColor.Yellow }); // 2
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Yellow });
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Yellow });
            gamestate.WinnerBets.Add(new GameEndBet() { Player = 6, CamelColor = CamelColor.Yellow });

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreGame(); // 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1

            Assert.AreEqual(4, gamestate.Money[0], "money p0");
            Assert.AreEqual(4, gamestate.Money[1], "money p1");
            Assert.AreEqual(5, gamestate.Money[2], "money p2");
            Assert.AreEqual(6, gamestate.Money[3], "money p3");
            Assert.AreEqual(3, gamestate.Money[4], "money p4");
            Assert.AreEqual(8, gamestate.Money[5], "money p5");
            Assert.AreEqual(4, gamestate.Money[6], "money p6");
            Assert.AreEqual(11, gamestate.Money[7], "money p7");
        }
        
        [TestMethod]
        public void RulesEngine_ScoreGame_Losers()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(8, startingPositions);

            gamestate.LoserBets.Add(new GameEndBet() { Player = 7, CamelColor = CamelColor.Blue }); // 8
            gamestate.LoserBets.Add(new GameEndBet() { Player = 5, CamelColor = CamelColor.Blue }); // 5
            gamestate.LoserBets.Add(new GameEndBet() { Player = 3, CamelColor = CamelColor.Blue }); // 3
            gamestate.LoserBets.Add(new GameEndBet() { Player = 2, CamelColor = CamelColor.Blue }); // 2
            gamestate.LoserBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Blue });
            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Blue });
            gamestate.LoserBets.Add(new GameEndBet() { Player = 6, CamelColor = CamelColor.Blue });

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreGame(); // 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1

            Assert.AreEqual(4, gamestate.Money[0], "money p0");
            Assert.AreEqual(4, gamestate.Money[1], "money p1");
            Assert.AreEqual(5, gamestate.Money[2], "money p2");
            Assert.AreEqual(6, gamestate.Money[3], "money p3");
            Assert.AreEqual(3, gamestate.Money[4], "money p4");
            Assert.AreEqual(8, gamestate.Money[5], "money p5");
            Assert.AreEqual(4, gamestate.Money[6], "money p6");
            Assert.AreEqual(11, gamestate.Money[7], "money p7");
        }
        
        [TestMethod]
        public void RulesEngine_ScoreGame_WrongBets()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Green });
            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Orange });
            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Red });

            gamestate.LoserBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Orange });
            gamestate.LoserBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Red });

            gamestate.LoserBets.Add(new GameEndBet() { Player = 2, CamelColor = CamelColor.Red });

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreGame();

            Assert.AreEqual(0, gamestate.Money[0], "money p0");
            Assert.AreEqual(1, gamestate.Money[1], "money p1");
            Assert.AreEqual(2, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }
    }
}
