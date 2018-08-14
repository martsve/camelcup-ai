using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;

namespace MyTestProgram
{
    class Program
    {
        const double TimeScalingFactor = 1.0;

        static void Main(string[] args)
        {
            var runner = new CamelRunner(2061097081);
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new RandomBot());

            var game = runner.ComputeSeededGame(1808123255, 92884888, 1278662542);
            Console.ReadLine();       
        }

        private static void RunBenchmark(string[] args)
        {
            var runner = new CamelRunner(timeScalingFactor: TimeScalingFactor);
            runner.AddPlayer(new DiceThrower());
            runner.AddPlayer(new IllegalBot());
            runner.AddPlayer(new RandomBot());            

            var history = runner.GetPlayers().ToDictionary(x => x, x => new List<TimeSpan>());

            for (var i = 0; i < 250; i++)
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
            
        }
    }
}
