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
        const double timeScalingFactor = 1.0;
        static TimeSpan MaxComputeTimePerAction = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);
        static TimeSpan MaxComputeTimePerGame = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);

        static void Main(string[] args)
        {         
            List<Player> Players = new List<Player>() 
            {
                new Player(new DiceThrower(), MaxComputeTimePerAction, MaxComputeTimePerGame),
                new Player(new MartinPlayer(), MaxComputeTimePerAction, MaxComputeTimePerGame),
                new Player(new RandomBot(), MaxComputeTimePerAction, MaxComputeTimePerGame),
                new Player(new IllegalBot(), MaxComputeTimePerAction, MaxComputeTimePerGame)
            };
            
            foreach (var player in Players)
            {
                player.PerformAction(x => x.Load());
            }

            var startPos = GetRandomStartingPositions();
            var game = new CamelCupGame(Players, startPos);

            game.StartGame();
            CamelHelper.Echo(game.ToString() + "\n--------------------------");
            
            while (!game.IsComplete())
            {
                game.MoveNextPlayer();

                CamelHelper.Echo(game.ToString() + "\n--------------------------");
                Console.ReadLine();
            }

            foreach (var player in Players)
            {
                player.PerformAction(x => x.Save());
            }            

            Console.ReadLine();
        }

        private static Dictionary<CamelColor, int> GetRandomStartingPositions() 
        {
            var rnd =  new Random();
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => rnd.Next(0, 2));
        }
    }
}
