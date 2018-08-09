using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;
using System.IO;

namespace Delver.CamelCup
{
    public class CamelRunner
    {
        public TimeSpan MaxComputeTimePerAction { get; set; }
        public TimeSpan MaxComputeTimePerGame { get; set; }

        public int Step { get; set; }

        private Random rnd;

        private List<Player> Players = new List<Player>();

        public CamelRunner(int seed = -1, double TimeScalingFactor = 1.0)
        {
            MaxComputeTimePerAction = TimeSpan.FromMilliseconds(1000 * TimeScalingFactor);
            MaxComputeTimePerGame = TimeSpan.FromMilliseconds(1000 * TimeScalingFactor);

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            rnd = new Random(seed);
        }

        public void AddPlayer(ICamelCupPlayer playerInterface) 
        {
            var player = new Player(playerInterface, MaxComputeTimePerAction, MaxComputeTimePerGame);
            player.PerformAction(x => x.Load());
            Players.Add(player);
        }

        public void LoadPlayers(string filename)
        {
            var lines = File.ReadAllLines("players.txt").Where(x => x.Trim().Length > 0).ToList();
            var interfaces = lines.Select(x => PlayerInterfaceFactory.CreateByName(x));

            foreach (var playerInterface in interfaces.Where(x => x == null)) 
            {
                throw new Exception($"Unable to create object");
            }

            foreach (var player in interfaces)
            {
                AddPlayer(player);
            }
        }

        public IEnumerable<Player> GetPlayers() 
        {
            return Players.ToList();
        }

        public CamelCupGame ComputeNewGame(int steps = -1)
        {
            var startPos = GetRandomStartingPositions();
            Players = GetRandomPlayerOrder(Players);
            var game = new CamelCupGame(Players, startPos, rnd.Next());

            game.StartGame();

            Step = 0;
            while (!game.IsComplete() && (steps < 0 || Step < steps))
            {
                game.MoveNextPlayer();
                CamelHelper.Echo(game);
                CamelHelper.Pause();
                Step++;
            }

            foreach (var winner in CamelHelper.GetWinners(game.GameState).Select(x => Players[x]))
            {
                winner.Wins++;
            }

            return game;
        }

        private Dictionary<CamelColor, Position> GetRandomStartingPositions() 
        {
            int i = 0;
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => new Position { Location = rnd.Next(0, 2), Height = i++ });
        }

        private List<Player> GetRandomPlayerOrder(List<Player> players)
        {
            return players.OrderBy(x => rnd.Next()).ToList();
        }
    }
}
