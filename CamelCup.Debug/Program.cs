using System;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;

using CamelCup.Test;
using Delver.CamelCup.External;
using System.Collections.Generic;

namespace CamelCup.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var players = new List<Player> () {
                GetTestBot(2),
                GetTestBot(1),
                GetTestBot(3),
                GetTestBot(4),
            };
            var game = new CamelCupGame(players, startingPositions, 1);
        }

        private static Player GetTestBot(int id)
        {
            return new Player(new RandomBot(id), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        static void N()
        {
            // 1: blue, green, orange | 2: red, yellow
            var startString = "1,0 1,1 1,2 2,3 2,4";
            var startingPositions = TestHelper.ConvertToStartingPositions(startString);
            var gamestate = new ImplementedGameState(1, startingPositions);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var wins = CamelHelper.GetCamelWins(gamestate, out int[] money);
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
