using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Delver.CamelCup.External;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestGetwinners
    {
        [TestMethod]
        public void RulesEngine_GetWinners_Base()
        {
            var gamestate = Setup();
            Assert.AreEqual("0 1 2 3", GetWinnerString(gamestate));
        }

        [TestMethod]
        public void RulesEngine_GetWinners_Money()
        {
            var gamestate = Setup();
            gamestate.Money = new Dictionary<int, int> { 
                { 0, 1 },
                { 1, 5 },
                { 2, 4 },
                { 3, 3 },
            };
            Assert.AreEqual("1", GetWinnerString(gamestate));
        }
        
        [TestMethod]
        public void RulesEngine_GetWinners_MoneyShared()
        {
            var gamestate = Setup();
            gamestate.Money = new Dictionary<int, int> { 
                { 0, 1 },
                { 1, 5 },
                { 2, 5 },
                { 3, 3 },
            };
            Assert.AreEqual("1 2", GetWinnerString(gamestate));
        }

        private GameState Setup()
        {
            // 1: blue | 2: green, white | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            return gamestate;
        }

        private string GetWinnerString(GameState gamestate)
        {
            var engine = new RulesEngine(gamestate, seed: 1);
            return string.Join(" ", engine.GetWinners().OrderBy(x => x));
        }
    }
}
