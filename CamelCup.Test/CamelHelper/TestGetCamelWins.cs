using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;

namespace CamelCup.Test
{
    [TestClass]
    public class TestGetCamelWins
    {
        [TestMethod]
        public void CamelHelper_GetCamelWins_NoTraps()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var wins = CamelHelper.GetCamelWins(gamestate);
            
            Assert.AreEqual(3107, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(6226, wins[CamelColor.Green], "green");
            Assert.AreEqual(9329, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(3302, wins[CamelColor.Red], "red");
            Assert.AreEqual(7196, wins[CamelColor.Yellow], "yellow");
        }
    }
}
