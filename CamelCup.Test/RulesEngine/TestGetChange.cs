using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Services.Test
{
    [TestClass]
    public class TestGetChange
    {
        [TestMethod]
        public void RulesEngine_ThrowDice_NoAction()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            var engine = new RulesEngine(gamestate, seed: 1);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.NoAction });

            Assert.IsTrue(change is NoActionStateChange);
        }

        [TestMethod]
        public void RulesEngine_ThrowDice_ThrowDice()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            var engine = new RulesEngine(gamestate, seed: 1);
            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.ThrowDice }) as DiceThrowStateChange;

            Assert.IsTrue(change != null, "is dice throw");
            Assert.AreEqual(CamelColor.Green, change.Color, "dice color");
            Assert.AreEqual(1, change.Value, "dice value");
            Assert.AreEqual(1, change.Player, "player");
        }
        
        [TestMethod]
        public void RulesEngine_ThrowDice_ThrowDice_NoDiceLeft()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(4, startingPositions);
            gamestate.RemainingDice.Clear();

            var engine = new RulesEngine(gamestate, seed: 1);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.ThrowDice });

            Assert.IsTrue(change == null);
        }
        
        [TestMethod]
        public void RulesEngine_ThrowDice_ThrowNonGreen()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            gamestate.RemainingDice.Remove(CamelColor.Green);

            var engine = new RulesEngine(gamestate, seed: 1);
            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.ThrowDice }) as DiceThrowStateChange;

            Assert.IsTrue(change != null, "is dice throw");
            Assert.AreEqual(CamelColor.Blue, change.Color, "dice color");
            Assert.AreEqual(1, change.Value, "dice value");
            Assert.AreEqual(1, change.Player, "player");
        }
        
        [TestMethod]
        public void RulesEngine_ThrowDice_ThrowRed()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            gamestate.RemainingDice.RemoveAll(x => x != CamelColor.Red);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.ThrowDice }) as DiceThrowStateChange;

            Assert.IsTrue(change != null, "is dice throw");
            Assert.AreEqual(CamelColor.Red, change.Color, "dice color");
            Assert.AreEqual(2, change.Value, "dice value");
            Assert.AreEqual(1, change.Player, "player");
        }
        
        [TestMethod]
        public void RulesEngine_PickCard_Valid5()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PickCard, Color = CamelColor.Red }) as PickCardStateChange;

            Assert.IsTrue(change != null, "is pick card");
            Assert.AreEqual(CamelColor.Red, change.Color, "card color");
            Assert.AreEqual(0, change.Player, "player");
            Assert.AreEqual(5, change.Value, "card value");
        }

        [TestMethod]
        public void RulesEngine_PickCard_Valid2()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.BettingCards.RemoveAll(x => x.CamelColor == CamelColor.Red && x.Value != 2);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PickCard, Color = CamelColor.Red }) as PickCardStateChange;

            Assert.IsTrue(change != null, "is pick card");
            Assert.AreEqual(CamelColor.Red, change.Color, "card color");
            Assert.AreEqual(0, change.Player, "player");
            Assert.AreEqual(2, change.Value, "card value");
        }       
        
        [TestMethod]
        public void RulesEngine_PickCard_Invalid()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.BettingCards.RemoveAll(x => x.CamelColor == CamelColor.Red);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PickCard, Color = CamelColor.Red });

            Assert.IsTrue(change == null, "is illegal");
        }

        // Test PlaceMinus
        [TestMethod]
        public void RulesEngine_PlaceMinus_AtStart()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 0 });

            Assert.IsTrue(change == null, "illegal space");
        }

        [TestMethod]
        public void RulesEngine_PlaceMinus_AtCamel()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 1 });
            Assert.IsTrue(change == null, "illegal space 1");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 2 });
            Assert.IsTrue(change == null, "illegal space 2");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 3 });
            Assert.IsTrue(change == null, "illegal space 3");
        }

        [TestMethod]
        public void RulesEngine_PlaceMinus_OutOfBounds()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = -1 });
            Assert.IsTrue(change == null, "illegal space 1");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 16 });
            Assert.IsTrue(change == null, "illegal space 16");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 100 });
            Assert.IsTrue(change == null, "illegal space 100");
        }
                
        [TestMethod]
        public void RulesEngine_PlaceMinus_OnOtherTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[1].Location = 5;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 4 });
            Assert.IsTrue(change == null, "illegal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 5 });
            Assert.IsTrue(change == null, "illegal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 6 });
            Assert.IsTrue(change == null, "illegal space 6");
        }
               
        [TestMethod]
        public void RulesEngine_PlaceMinus_OnOwnTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[0].Location = 5;
            gamestate.Traps[0].Move = 1;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 4 });
            Assert.IsTrue(change != null, "legal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 5 });
            Assert.IsTrue(change != null, "legal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 6 });
            Assert.IsTrue(change != null, "legal space 6");
        }

        [TestMethod]
        public void RulesEngine_PlaceMinus_OnOwnMinusTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[0].Location = 5;
            gamestate.Traps[0].Move = -1;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 4 });
            Assert.IsTrue(change != null, "legal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 5 });
            Assert.IsTrue(change == null, "illegal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 6 });
            Assert.IsTrue(change != null, "legal space 6");
        }

        [TestMethod]
        public void RulesEngine_PlaceMinus_ValidSpace()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = 4 }) as MinusTrapStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player);
            Assert.AreEqual(4, change.Value);
        }
        
        [TestMethod]
        public void RulesEngine_PlacePlus_AtStart()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 0 });

            Assert.IsTrue(change == null, "illegal space");
        }

        [TestMethod]
        public void RulesEngine_PlacePlus_AtCamel()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 1 });
            Assert.IsTrue(change == null, "illegal space 1");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 2 });
            Assert.IsTrue(change == null, "illegal space 2");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 3 });
            Assert.IsTrue(change == null, "illegal space 3");
        }

        [TestMethod]
        public void RulesEngine_PlacePluss_OutOfBounds()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = -1 });
            Assert.IsTrue(change == null, "illegal space 1");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 16 });
            Assert.IsTrue(change == null, "illegal space 16");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 100 });
            Assert.IsTrue(change == null, "illegal space 100");
        }
                
        [TestMethod]
        public void RulesEngine_PlacePluss_OnOtherTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[1].Location = 5;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 4 });
            Assert.IsTrue(change == null, "illegal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 5 });
            Assert.IsTrue(change == null, "illegal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 6 });
            Assert.IsTrue(change == null, "illegal space 6");
        }
               
        [TestMethod]
        public void RulesEngine_PlacePluss_OnOwnTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[0].Location = 5;
            gamestate.Traps[0].Move = -1;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 4 });
            Assert.IsTrue(change != null, "legal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 5 });
            Assert.IsTrue(change != null, "legal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 6 });
            Assert.IsTrue(change != null, "legal space 6");
        }

        [TestMethod]
        public void RulesEngine_PlacePluss_OnOwnPlussTrap()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.Traps[0].Location = 5;
            gamestate.Traps[0].Move = 1;
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 4 });
            Assert.IsTrue(change != null, "legal space 4");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 5 });
            Assert.IsTrue(change == null, "illegal space 5");

            change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 6 });
            Assert.IsTrue(change != null, "legal space 6");
        }

        [TestMethod]
        public void RulesEngine_PlacePluss_ValidSpace()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = 4 }) as PlussTrapStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player);
            Assert.AreEqual(4, change.Value);
        }

        [TestMethod]
        public void RulesEngine_BetOnLoser_Valid()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, Color = CamelColor.Green }) as LoserBetStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player, "player");
            Assert.AreEqual(CamelColor.Green, change.Color, "bet on green");
        }

        [TestMethod]
        public void RulesEngine_BetOnLoser_Valid_Other()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Green });
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, Color = CamelColor.Green }) as LoserBetStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player, "player");
            Assert.AreEqual(CamelColor.Green, change.Color, "bet on green");
        }

        [TestMethod]
        public void RulesEngine_BetOnLoser_Invalid_Self()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.LoserBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Green });
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, Color = CamelColor.Green });
            Assert.IsTrue(change == null, "is invalid");
        }
        
        [TestMethod]
        public void RulesEngine_BetOnWinner_Valid()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, Color = CamelColor.Green }) as WinnerBetStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player, "player");
            Assert.AreEqual(CamelColor.Green, change.Color, "bet on green");
        }

        [TestMethod]
        public void RulesEngine_BetOnWinner_Valid_Other()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.LoserBets.Add(new GameEndBet() { Player = 0, CamelColor = CamelColor.Green });
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, Color = CamelColor.Green }) as WinnerBetStateChange;
            Assert.IsTrue(change != null, "is valid");
            Assert.AreEqual(1, change.Player, "player");
            Assert.AreEqual(CamelColor.Green, change.Color, "bet on green");
        }

        [TestMethod]
        public void RulesEngine_BetOnWinner_Invalid_Self()
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);
            gamestate.LoserBets.Add(new GameEndBet() { Player = 1, CamelColor = CamelColor.Green });
            var engine = new RulesEngine(gamestate, seed: 2);

            var change = engine.Getchange(1, new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, Color = CamelColor.Green });
            Assert.IsTrue(change == null, "is invalid");
        }

    }
}
