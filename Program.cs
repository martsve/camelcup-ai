using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;

namespace Delver.CamelCup
{
    class Program
    {
        static TimeSpan MaxComputeTimePerAction = TimeSpan.FromMilliseconds(1000);
        static TimeSpan MaxComputeTimePerGame = TimeSpan.FromMilliseconds(1000);

        static void DebugWrite(string str) 
        {
            if (true) {
                Console.WriteLine(str);
                Console.WriteLine("--------------------------");
            }
        }

        static void Main(string[] args)
        {         
            List<Player> Players = new List<Player>() 
            {
                new Player(new DiceThrower(), MaxComputeTimePerAction, MaxComputeTimePerGame),
                new Player(new MartinPlayer(), MaxComputeTimePerAction, MaxComputeTimePerGame),
                new Player(new RandomBot(), MaxComputeTimePerAction, MaxComputeTimePerGame)
            };
            
            foreach (var player in Players)
            {
                player.PerformAction(x => x.Load());
            }

            var startPos = GetRandomStartingPositions();
            var game = new CamelCupGame(Players, startPos);

            game.StartGame();
            DebugWrite(game.ToString());
            
            while (!game.IsComplete())
            {
                game.MoveNextPlayer();

                DebugWrite(game.ToString());
                Console.ReadLine();
            }

            foreach (var player in Players)
            {
                player.PerformAction(x => x.Save());
            }            
        }

        private static Dictionary<CamelColor, int> GetRandomStartingPositions() 
        {
            var rnd =  new Random();
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => rnd.Next(0, 2));
        }
    }
}
