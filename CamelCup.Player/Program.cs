using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup;
using Delver.CamelCup.External;

namespace MyTestProgram
{
    class Program
    {
        const double timeScalingFactor = 1.0;

        static void Main(string[] args)
        {
            var filename = args.FirstOrDefault(x => !x.StartsWith("-")) ?? "players.txt";

            var runner = new CamelRunner(TimeScalingFactor: timeScalingFactor);
            runner.LoadPlayers(filename);

            var history = runner.GetPlayers().ToDictionary(x => x, x => new List<TimeSpan>());

            for (int i = 0; i < 250; i++)
            {
                runner.ComputeNewGame();
                Console.WriteLine($"Game #{i} finished");

                foreach (var player in runner.GetPlayers())
                    history[player].Add(player.ComputationTime);
            } 

            runner.Save();

            var players = runner.GetPlayers();

            var title = "Player";
            Console.WriteLine($"{title,-32} Wins");
            foreach (var player in players.OrderByDescending(x => x.Wins))
            {
                var avgTime = history[player].Average(x => x.TotalSeconds);
                Console.WriteLine($"{player.Name,-32} {player.Wins, -6} {avgTime:0.000}s");
            }    
            
            Console.ReadLine();       
        }
    }
}
