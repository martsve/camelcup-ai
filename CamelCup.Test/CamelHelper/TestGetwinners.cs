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
    public class TestGetwinners
    {
        [TestMethod]
        public void CamelHelper_GetWinners_Base()
        {
            var gamestate = Setup();
            Assert.AreEqual("0 1 2 3", GetWinnerString(gamestate));
        }

        [TestMethod]
        public void CamelHelper_GetWinners_Money()
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
        public void CamelHelper_GetWinners_MoneyShared()
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
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            return gamestate;
        }

        private string GetWinnerString(GameState gamestate)
        {
            return string.Join(" ", CamelHelper.GetWinners(gamestate).OrderBy(x => x));
        }
    }
}
