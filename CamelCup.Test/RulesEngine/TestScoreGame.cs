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
    public class TestScoreGame
    {
        [TestMethod]
        public void RulesEngine_ScoreGame()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 1);
            engine.ScoreGame();
        }
    }
}
