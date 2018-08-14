using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestThrowDiceStateChange
    {
        [TestMethod]
        public void Unit_State_DiceThrow()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 2,3 2,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 2: red, yellow, blue, green, orange
            gamestate.Apply(change);

            Assert.AreEqual("2,2 2,3 2,4 2,0 2,1", gamestate.CamelPositionToString(), "Camel positions");

            Assert.AreEqual(4, gamestate.Money[0], "money");
        }

       [TestMethod]
        public void Unit_State_DiceThrow_Trap1()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = 1;
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 3: red, yellow, blue, green, orange
            gamestate.Apply(change);

            Assert.AreEqual("3,2 3,3 3,4 3,0 3,1", gamestate.CamelPositionToString(), "Camel positions");

            Assert.AreEqual(5, gamestate.Money[0], "money");
        }
        
       [TestMethod]
        public void Unit_State_DiceThrow_Trap2()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Blue, 1);
            
            // 3: red, yellow, blue, green, orange
            gamestate.Apply(change);

            Assert.AreEqual("1,0 1,1 1,2 3,3 3,4", gamestate.CamelPositionToString(), "Camel positions");

            Assert.AreEqual(5, gamestate.Money[0], "money");
        }
                
       [TestMethod]
        public void Unit_State_DiceThrow_Trap3()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[0].Location = 2;
            gamestate.Traps[0].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Green, 1);
            
            // 1: green, orange, BLUE | trap | 3: red, yellow
            gamestate.Apply(change);

            Assert.AreEqual("1,2 1,0 1,1 3,3 3,4", gamestate.CamelPositionToString(), "Camel positions");

            Assert.AreEqual(5, gamestate.Money[0], "money");
        }

        [TestMethod]
        public void Unit_State_DiceThrow_Trap4()
        {
            // 1: blue, green, orange | trap | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            gamestate.Traps[1].Location = 2;
            gamestate.Traps[1].Move = -1;
            var change = new DiceThrowStateChange(0, CamelColor.Green, 1);
            
            // 1: green, orange, BLUE | trap | 3: red, yellow
            gamestate.Apply(change);

            Assert.AreEqual("1,2 1,0 1,1 3,3 3,4", gamestate.CamelPositionToString(), "Camel positions");

            Assert.AreEqual(4, gamestate.Money[0], "money p0");
            Assert.AreEqual(4, gamestate.Money[1], "money p1");
        }
    }
}
