using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;

namespace CamelCup.Test
{
    [TestClass]
    public class TestGetAllGameStates
    {
        private static string[] depth0positions = File.ReadAllLines("ExpectedResults/Positions_Depth_0.txt").Select(x => x.Trim()).ToArray(); 
        private static string[] depth1positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_1.txt").Select(x => x.Trim()).ToArray(); 
        private static string[] depth2positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_2.txt").Select(x => x.Trim()).ToArray(); 
        private static string[] depth3positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_3.txt").Select(x => x.Trim()).ToArray(); 
        private static string[] depth4positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_4.txt").Select(x => x.Trim()).ToArray(); 

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_0()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 0);

            Assert.AreEqual(15, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth0positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_1()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 1);

            Assert.AreEqual(15*12, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth0positions)
                Assert.IsTrue(!positions.Contains(pos), "Found state: " + pos);

            foreach (var pos in depth1positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_0_and_1()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 1, includeAllStates: true);

            Assert.AreEqual(15*12 + 15, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth0positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);

            foreach (var pos in depth1positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_2()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 2);

            Assert.AreEqual(15*12*9, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth1positions)
                Assert.IsTrue(!positions.Contains(pos), "Found depth 1 state: " + pos);

            foreach (var pos in depth2positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_3()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 3);

            Assert.AreEqual(15*12*9*6, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth3positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_4()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 4);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth4positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_5()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = CamelHelper.GetAllCamelEndStates(gamestate, 5);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");

            var positions = states.Select(x => TestHelper.CamelPositionToString(x)).ToList();

            foreach (var pos in depth4positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
    }
}
