using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestGetCamelWins
    {
        [TestMethod]
        public void CamelHelper_GetCamelWins_SimpleWin_Stacked()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            gamestate.RemainingDice.RemoveAll(x => x != CamelColor.Red);

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(0, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(0, wins[CamelColor.Green], "green");
            Assert.AreEqual(0, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(0, wins[CamelColor.Red], "red");
            Assert.AreEqual(3, wins[CamelColor.Yellow], "yellow");
        }

        [TestMethod]
        public void CamelHelper_GetCamelWins_SimpleWin()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            gamestate.RemainingDice.RemoveAll(x => x != CamelColor.Yellow);

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(0, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(0, wins[CamelColor.Green], "green");
            Assert.AreEqual(0, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(0, wins[CamelColor.Red], "red");
            Assert.AreEqual(3, wins[CamelColor.Yellow], "yellow");
        }

        [TestMethod]
        public void CamelHelper_GetCamelWins_SimpleWin_TrapMinus()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            gamestate.Traps[0].Location = 3;
            gamestate.Traps[0].Move = -1;

            gamestate.RemainingDice.RemoveAll(x => x != CamelColor.Green);

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(0, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(0, wins[CamelColor.Green], "green");
            Assert.AreEqual(2, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(0, wins[CamelColor.Red], "red");
            Assert.AreEqual(1, wins[CamelColor.Yellow], "yellow");

            Assert.AreEqual(1, money[3], "trap money");
        }
        
        [TestMethod]
        public void CamelHelper_GetCamelWins_SimpleWin_TrapPlus()
        {
            // 1: blue, green, orange | 4: red, yellow
            var startString = "1,0 1,1 1,2 4,3 4,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            gamestate.Traps[0].Location = 3;
            gamestate.Traps[0].Move = 1;

            gamestate.RemainingDice.RemoveAll(x => x != CamelColor.Green);

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(0, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(0, wins[CamelColor.Green], "green");
            Assert.AreEqual(2, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(0, wins[CamelColor.Red], "red");
            Assert.AreEqual(1, wins[CamelColor.Yellow], "yellow");

            Assert.AreEqual(1, money[3], "trap money");
        }

        [TestMethod]
        public void CamelHelper_GetCamelWins_Full_NoTraps()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(3107, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(6226, wins[CamelColor.Green], "green");
            Assert.AreEqual(9329, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(3302, wins[CamelColor.Red], "red");
            Assert.AreEqual(7196, wins[CamelColor.Yellow], "yellow");
        }

        [TestMethod]
        public void CamelHelper_GetCamelWins_Full_PlussTrap()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            gamestate.Traps[0].Location = 3;
            gamestate.Traps[0].Move = 1;

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(4239, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(6215, wins[CamelColor.Green], "green");
            Assert.AreEqual(8217, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(3983, wins[CamelColor.Red], "red");
            Assert.AreEqual(6506, wins[CamelColor.Yellow], "yellow");

            Assert.AreEqual(34263, money[3], "trap money");
        }

        [TestMethod]
        public void CamelHelper_GetCamelWins_Full_MinusTrap()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            gamestate.Traps[0].Location = 3;
            gamestate.Traps[0].Move = -1;

            var wins = CamelHelper.GetCamelWins(gamestate, out var money);
            
            Assert.AreEqual(3086, wins[CamelColor.Blue], "blue");
            Assert.AreEqual(5240, wins[CamelColor.Green], "green");
            Assert.AreEqual(8834, wins[CamelColor.Orange], "orange");
            Assert.AreEqual(3251, wins[CamelColor.Red], "red");
            Assert.AreEqual(8749, wins[CamelColor.Yellow], "yellow");

            Assert.AreEqual(38500, money[3], "trap money");
        }
    }
}
