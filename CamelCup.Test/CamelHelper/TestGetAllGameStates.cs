using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestGetAllGameStates
    {
        private static readonly string[] _depth0Positions = File.ReadAllLines("ExpectedResults/Positions_Depth_0.txt").Select(x => x.Trim()).ToArray(); 
        private static readonly string[] _depth1Positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_1.txt").Select(x => x.Trim()).ToArray(); 
        private static readonly string[] _depth2Positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_2.txt").Select(x => x.Trim()).ToArray(); 
        private static readonly string[] _depth3Positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_3.txt").Select(x => x.Trim()).ToArray(); 
        private static readonly string[] _depth4Positions  = File.ReadAllLines("ExpectedResults/Positions_Depth_4.txt").Select(x => x.Trim()).ToArray(); 

        private string CamelsToString(List<Camel> camels)
        {
            return string.Join(" ", camels.Select(x => $"{x.Location},{x.Height}"));
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_0()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(0);

            Assert.AreEqual(15, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth0Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_1()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(1);

            Assert.AreEqual(15*12, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth0Positions)
                Assert.IsTrue(!positions.Contains(pos), "Found state: " + pos);

            foreach (var pos in _depth1Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_0_and_1()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(1, includeAllStates: true);

            Assert.AreEqual(15*12 + 15, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth0Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);

            foreach (var pos in _depth1Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_2()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(2);

            Assert.AreEqual(15*12*9, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth1Positions)
                Assert.IsTrue(!positions.Contains(pos), "Found depth 1 state: " + pos);

            foreach (var pos in _depth2Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_3()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(3);

            Assert.AreEqual(15*12*9*6, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth3Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_4()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(4);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth4Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }

                
        [TestMethod]
        public void CamelHelper_GetallGameStates_Depth_5()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);
            var states = gamestate.GetAllGameEndStates(5);

            Assert.AreEqual(15*12*9*6*3, states.Count, "final states");

            var positions = states.Select(CamelsToString).ToList();

            foreach (var pos in _depth4Positions)
                Assert.IsTrue(positions.Contains(pos), "Can't find state: " + pos);
        }
    }
}
