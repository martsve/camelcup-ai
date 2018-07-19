using System;
using System.Collections.Generic;
using System.Linq;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    class Program
    {
        const double timeScalingFactor = 1.0;

        static void Main(string[] args)
        {
            CamelHelper.StepTrough = args.Any(x => x == "-d");
            var filename = args.FirstOrDefault(x => !x.StartsWith("-")) ?? "players.txt";

            var runner = new CamelRunner(TimeScalingFactor: timeScalingFactor);
            runner.LoadPlayers(filename);

            for (int i = 0; i < 1000; i++)
            {
                runner.ComputeNewGame();
                Console.WriteLine($"Game #{i} finished");
            } 

            var players = runner.GetPlayers();

            var title = "Player";
            Console.WriteLine($"{title,-32} Wins");
            foreach (var player in players.OrderByDescending(x => x.Wins))
            {
                Console.WriteLine($"{player.Name,-32} {player.Wins}");
            }    
            
            Console.ReadLine();       
        }
    }
}
