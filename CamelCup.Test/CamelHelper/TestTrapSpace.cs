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
    public class TestTrapSpace
    {
        [TestMethod]
        public void CamelHelper_TrapSpace_IsValid_Pos0Invalid()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 0, 0), "valid at pos 0");
        }

        [TestMethod]
        public void CamelHelper_TrapSpace_IsValid_NonOccupied()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            
            for (int i = 4; i < 16; i++)
                Assert.AreEqual(true,  CamelHelper.IsValidTrapSpace(gamestate, 0, i), $"valid at pos {i}");
        }

        [TestMethod]
        public void CamelHelper_TrapSpace_IsValid_Occupied()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 0, 1), "Invalid at pos 1");
            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 0, 2), "Invalid at pos 2");
            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 0, 3), "Invalid at pos 3");
        }
        
        [TestMethod]
        public void CamelHelper_TrapSpace_IsValid_EnemyTrap()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.Traps[0].Location = 5;

            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 1, 4), "Invalid at pos 4");
            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 1, 5), "Invalid at pos 5");
            Assert.AreEqual(false,  CamelHelper.IsValidTrapSpace(gamestate, 1, 6), "Invalid at pos 6");
        }
  
        [TestMethod]
        public void CamelHelper_TrapSpace_IsValid_OwnTrap()
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);

            gamestate.Traps[1].Location = 5;

            Assert.AreEqual(true,  CamelHelper.IsValidTrapSpace(gamestate, 1, 4), "valid at pos 4");
            Assert.AreEqual(true,  CamelHelper.IsValidTrapSpace(gamestate, 1, 5), "valid at pos 5");
            Assert.AreEqual(true,  CamelHelper.IsValidTrapSpace(gamestate, 1, 6), "valid at pos 6");
        }
    }
}
