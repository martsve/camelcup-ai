using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    class Program
    {
        const double timeScalingFactor = 1.0;
        static TimeSpan MaxComputeTimePerAction = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);
        static TimeSpan MaxComputeTimePerGame = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);

        static Player PlayerFactory(ICamelCupPlayer player) 
        {
            return new Player(player, MaxComputeTimePerAction, MaxComputeTimePerGame);
        }

        static void Main(string[] args)
        {         
            List<Player> Players = new List<Player>() 
            {
                PlayerFactory(new DiceThrower()),
                PlayerFactory(new MartinPlayer()),
                PlayerFactory(new RandomBot()),
                PlayerFactory(new SmartMartinPlayer())
            };
            
            foreach (var player in Players)
            {
                player.PerformAction(x => x.Load());
            }

            for (int i = 0; i < 1000; i++)
            {
                var startPos = GetRandomStartingPositions();
                var randomPlayerOrder = GetRandomPlayerOrder(Players);
                var game = new CamelCupGame(randomPlayerOrder, startPos);

                game.StartGame();

                while (!game.IsComplete())
                {
                    game.MoveNextPlayer();
                }

                foreach (var winner in CamelHelper.GetWinners(game.GameState).Select(x => randomPlayerOrder[x]))
                {
                    winner.Wins++;
                }

                Console.WriteLine($"Game #{i} finished");
            } 

            var title = "Player";
            Console.WriteLine($"{title,-32} Wins");
            foreach (var player in Players.OrderByDescending(x => x.Wins))
            {
                Console.WriteLine($"{player.Name,-32} {player.Wins}");
            }    
            
            Console.ReadLine();       
        }

        private static Dictionary<CamelColor, int> GetRandomStartingPositions() 
        {
            var rnd = new Random();
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => rnd.Next(0, 2));
        }

        private static List<Player> GetRandomPlayerOrder(List<Player> players)
        {
            var rnd = new Random();
            return players.OrderBy(x => rnd.Next()).ToList();
        }
    }
}
