using System;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;

using CamelCup.Test;
using Delver.CamelCup.External;

namespace CamelCup.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new CamelRunner(seed: 1);
            runner.AddPlayer(new RandomBot(1, seed: 2));
            runner.AddPlayer(new RandomBot(2, seed: 1));

            var game = runner.ComputeNewGame(steps: 0);
        }
    }
}
