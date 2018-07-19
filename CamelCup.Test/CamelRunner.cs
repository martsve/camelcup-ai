using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;

namespace CamelCup.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Integration_CamelRunner_Valid()
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1));
            runner.AddPlayer(new RandomBot(2));
            runner.ComputeNewGame();    
        }
    }
}
