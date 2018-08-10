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
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var wins = CamelHelper.GetCamelWins(gamestate);
            watch.Stop();

            Console.WriteLine(wins.Sum(x => x.Value));
            foreach (var win in wins)
            {
                Console.WriteLine($"{win.Key}: {win.Value}");
            }

            Console.WriteLine($"{watch.ElapsedMilliseconds} ms");
        }
    }
}
