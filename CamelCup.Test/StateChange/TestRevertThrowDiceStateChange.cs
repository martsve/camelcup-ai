using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.Collections.Generic;

namespace CamelCup.Test
{
    [TestClass]
    public class TestRevertThrowDiceStateChange
    {
        [TestMethod]
        public void Unit_RevertState_DiceThrow()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 2, 2);
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 2: red, yellow, blue, green, orange
            gamestate.Apply(change);
            change.Revert(gamestate);

            AssertBaseState(gamestate, startingPositions);
        }

       [TestMethod]
        public void Unit_RevertState_DiceThrow_Trap1()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 3, 3);
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = 1;
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 3: red, yellow, blue, green, orange
            gamestate.Apply(change);
            change.Revert(gamestate);

            AssertBaseState(gamestate, startingPositions);
        }
        
       [TestMethod]
        public void Unit_RevertState_DiceThrow_Trap2()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 3, 3);
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 3: red, yellow, blue, green, orange
            gamestate.Apply(change);
            change.Revert(gamestate);

            AssertBaseState(gamestate, startingPositions);
        }
                
       [TestMethod]
        public void Unit_RevertState_DiceThrow_Trap3()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 3, 3);
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Green, 1);
            
            // 1: green, orange, BLUE | trap | 3: red, yellow
            gamestate.Apply(change);
            change.Revert(gamestate);

            AssertBaseState(gamestate, startingPositions);
        }

        [TestMethod]
        public void Unit_RevertState_DiceThrow_Trap4()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions(1, 1, 1, 3, 3);
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[1].Location = 2;
            gamestate.Traps[1].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Green, 1);
            
            // 1: green, orange, BLUE | trap | 3: red, yellow
            gamestate.Apply(change);
            change.Revert(gamestate);

            AssertBaseState(gamestate, startingPositions);
        }

        private void AssertBaseState(GameState gamestate, Dictionary<CamelColor, Position> startPos)
        {
            foreach (var color in CamelHelper.GetAllCamelColors())
            {
                Assert.AreEqual(startPos[color], gamestate.GetPosition(color), color.ToString());
            }

            Assert.AreEqual(3, gamestate.Money[0], "money p0");
            Assert.AreEqual(3, gamestate.Money[0], "money p1");
        }
    }
}
