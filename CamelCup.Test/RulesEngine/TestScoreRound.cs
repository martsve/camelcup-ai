using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;
using System.Collections.Generic;

namespace CamelCup.Test
{
    [TestClass]
    public class TestScoreRound
    {
        [TestMethod]
        public void RulesEngine_ScoreRound_NoBets()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreRound();
            
            Assert.AreEqual(3, gamestate.Money[0], "money p0");
            Assert.AreEqual(3, gamestate.Money[1], "money p1");
            Assert.AreEqual(3, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }

       [TestMethod]
        public void RulesEngine_ScoreRound_Bets_Winner()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Yellow && x.Value == 5).Owner = 0;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Yellow && x.Value == 3).Owner = 1;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Yellow && x.Value == 2).Owner = 2;

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreRound();
            
            Assert.AreEqual(8, gamestate.Money[0], "money p0");
            Assert.AreEqual(6, gamestate.Money[1], "money p1");
            Assert.AreEqual(5, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }     
        
       [TestMethod]
        public void RulesEngine_ScoreRound_Bets_SecondPlace()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Red && x.Value == 5).Owner = 0;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Red && x.Value == 3).Owner = 1;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Red && x.Value == 2).Owner = 2;

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreRound();
            
            Assert.AreEqual(4, gamestate.Money[0], "money p0");
            Assert.AreEqual(4, gamestate.Money[1], "money p1");
            Assert.AreEqual(4, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }     
        
       [TestMethod]
        public void RulesEngine_ScoreRound_Bets_Loser()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Orange && x.Value == 5).Owner = 0;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Green && x.Value == 5).Owner = 0;

            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Orange && x.Value == 3).Owner = 1;
            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Blue && x.Value == 5).Owner = 1;

            gamestate.BettingCards.First(x => x.CamelColor == CamelColor.Orange && x.Value == 2).Owner = 2;

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreRound();
            
            Assert.AreEqual(1, gamestate.Money[0], "money p0");
            Assert.AreEqual(1, gamestate.Money[1], "money p1");
            Assert.AreEqual(2, gamestate.Money[2], "money p2");
            Assert.AreEqual(3, gamestate.Money[3], "money p3");
        }     
    }
}
