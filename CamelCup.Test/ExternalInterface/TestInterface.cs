using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.Collections.Generic;
using System;

namespace CamelCup.Test
{
    [TestClass]
    public class TestInterface
    {
        [TestMethod]
        public void Interface_Start()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            var game = runner.ComputeNewGame(steps: 0);

            Assert.AreEqual("RandomBot #1", bot.GameInfo.Players[1], "name");
            Assert.AreEqual("TestBot", bot.GameInfo.Players[0], "name");
            Assert.AreEqual(0, bot.PlayerNum, "testbot order");

            Assert.AreEqual(1, bot.CalledPlayerName, "player name");
            Assert.AreEqual(1, bot.CalledLoad, "load");
            Assert.AreEqual(1, bot.CalledStart, "start");

            Assert.AreEqual(0, bot.CalledAction, "action");
            Assert.AreEqual(0, bot.CalledInform, "inform");
            Assert.AreEqual(0, bot.CalledSave, "save");
            Assert.AreEqual(0, bot.CalledWinners, "winners");
        }
        
        [TestMethod]
        public void Interface_Call_OnceReverse()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 2);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            var game = runner.ComputeSeededGame(1, 3, 3, steps: 1);

            Assert.AreEqual("RandomBot #1", bot.GameInfo.Players[0], "name");
            Assert.AreEqual("TestBot", bot.GameInfo.Players[1], "name");
            
            Assert.AreEqual(1, bot.PlayerNum, "testbot order");
            Assert.AreEqual(1, bot.CalledPlayerName, "player name");
            Assert.AreEqual(1, bot.CalledLoad, "load");
            Assert.AreEqual(1, bot.CalledStart, "start");
            Assert.AreEqual(1, bot.CalledInform, "inform");

            Assert.AreEqual(0, bot.CalledAction, "action");
            Assert.AreEqual(0, bot.CalledSave, "save");
            Assert.AreEqual(0, bot.CalledWinners, "winners");
        }

        [TestMethod]
        public void Interface_Call_Once()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            var game = runner.ComputeNewGame(steps: 1);

            Assert.AreEqual(0, bot.PlayerNum, "testbot order");
            Assert.AreEqual(1, bot.CalledPlayerName, "player name");
            Assert.AreEqual(1, bot.CalledLoad, "load");
            Assert.AreEqual(1, bot.CalledStart, "start");
            Assert.AreEqual(1, bot.CalledAction, "action");
            Assert.AreEqual(1, bot.CalledInform, "inform");

            Assert.AreEqual(0, bot.CalledSave, "save");
            Assert.AreEqual(0, bot.CalledWinners, "winners");
        }

        [TestMethod]
        public void Interface_Call_Twice()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            var game = runner.ComputeNewGame(steps: 2);

            Assert.AreEqual(1, bot.CalledPlayerName, "player name");
            Assert.AreEqual(1, bot.CalledLoad, "load");
            Assert.AreEqual(1, bot.CalledStart, "start");
            Assert.AreEqual(1, bot.CalledAction, "action");
            Assert.AreEqual(2, bot.CalledInform, "inform");

            Assert.AreEqual(0, bot.CalledSave, "save");
            Assert.AreEqual(0, bot.CalledWinners, "winners");
        }

        [TestMethod]
        public void Interface_Call_WholeGame()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            var game = runner.ComputeSeededGame(1, 2, 3);

            Assert.AreEqual(1, bot.CalledPlayerName, "player name");
            Assert.AreEqual(1, bot.CalledLoad, "load");
            Assert.AreEqual(1, bot.CalledStart, "start");
            Assert.AreEqual(19, bot.CalledAction, "action");
            Assert.AreEqual(38, bot.CalledInform, "inform");
            Assert.AreEqual(1, bot.CalledWinners, "winners");

            Assert.AreEqual(0, bot.CalledSave, "save");
        }

        [TestMethod]
        public void Interface_Call_Save()
        {
            var bot = GetTestBot();

            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(bot);

            runner.ComputeNewGame(steps:0 );
            runner.Save();

            Assert.AreEqual(1, bot.CalledSave, "save");
        }

        private TestBot GetTestBot()
        {
            return new TestBot(x => 
            {
                return new PlayerAction() { CamelAction = CamelAction.ThrowDice };
            });
        }

        class TestBot : ICamelCupPlayer
        {
            Func<GameState, PlayerAction> func;

            public TestBot(Func<GameState, PlayerAction> func)
            {
                this.func = func;
            }

            public int CalledPlayerName;
            public int CalledLoad;
            public int CalledSave;
            public int CalledStart;
            public int CalledWinners;
            public int CalledInform;
            public int CalledAction;

            public GameInfo GameInfo;
            public int PlayerNum;

            public PlayerAction GetAction(GameState gameState)
            {
                CalledAction++;
                return func.Invoke(gameState);
            }

            public string GetPlayerName()
            {
                CalledPlayerName++;
                return "TestBot";
            }

            public void InformAboutAction(int player, PlayerAction action, GameState gameState)
            {
                if (player == PlayerNum && action.CamelAction != CamelAction.ThrowDice)
                    Assert.Fail("We threw dice");

                CalledInform++;
            }

            public void Load()
            {
                CalledLoad++;
            }

            public void Save()
            {
                CalledSave++;
            }

            public void StartNewGame(int playerId, GameInfo info, GameState gameState)
            {
                PlayerNum = playerId;
                GameInfo = info;
                CalledStart++;
            }

            public void Winners(List<int> winners, GameState gameState)
            {
                CalledWinners++;
            }
        }
    }
}
