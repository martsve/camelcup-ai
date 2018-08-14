using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestPickCardStateChange
    {
        [TestMethod]
        public void Unit_State_PickCard()
        {
            // 1: blue, green, white | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 2,3 2,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);
            var change = new PickCardStateChange(0, CamelColor.Blue, 5);            
            gamestate.Apply(change);

            var owner = gamestate.BettingCards.Where(x => x.Value == 5).ToDictionary(x => x.CamelColor, x => x.Owner);

            Assert.AreEqual(0, owner[CamelColor.Blue], "blue 5 owner");
            Assert.AreEqual(-1, owner[CamelColor.Green], "green 5 owner");
            Assert.AreEqual(-1, owner[CamelColor.White], "white 5 owner");
            Assert.AreEqual(-1, owner[CamelColor.Red], "red 5 owner");
            Assert.AreEqual(-1, owner[CamelColor.Yellow], "yellow 5 owner");            
        }

        [TestMethod]
        public void Unit_State_PickCard_Twice()
        {
            // 1: blue, green, white | 2: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 1,1 1,2 2,3 2,4");
            var gamestate = new ImplementedGameState(2, startingPositions, 16, 3);

            gamestate.Apply(new PickCardStateChange(0, CamelColor.Blue, 5));
            gamestate.Apply(new PickCardStateChange(0, CamelColor.Blue, 3));

            var c5 = gamestate.BettingCards.First(x => x.Value == 5 && x.CamelColor == CamelColor.Blue).Owner;
            var c3 = gamestate.BettingCards.First(x => x.Value == 3 && x.CamelColor == CamelColor.Blue).Owner;
            var c2 = gamestate.BettingCards.First(x => x.Value == 2 && x.CamelColor == CamelColor.Blue).Owner;

            Assert.AreEqual(0, c5, "blue 5 owner");
            Assert.AreEqual(0, c3, "blue 3 owner");
            Assert.AreEqual(-1, c2, "blue 2 owner");
        }
    }
}
