using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;

namespace CamelCup.Test
{
    [TestClass]
    public class TestGetAllGameStates
    {
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_0()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 0);

            Assert.AreEqual(15, states.Count, "final states");
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_1()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 1);

            Assert.AreEqual(15*12, states.Count, "final states");
        }
        
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_2()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 2);

            Assert.AreEqual(15*12*9, states.Count, "final states");
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_3()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 3);

            Assert.AreEqual(15*12*9*6, states.Count, "final states");
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_4()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 4);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");
        }

                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_5()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 5);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");
        }
    }
}
