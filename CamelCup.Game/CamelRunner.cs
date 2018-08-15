using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;
using System.IO;

namespace Delver.CamelCup
{
    public class CamelRunner
    {
        public TimeSpan MaxComputeTimePerAction { get; set; }
        public TimeSpan MaxComputeTimePerGame { get; set; }

        public int Step { get; set; }

        private Random _rnd;

        private List<Player> _players = new List<Player>();
        
        public int? StartPositionSeed;
        public int? PlayerOrderSeed;
        public int? GameSeed;

        public CamelRunner(int seed = -1, double timeScalingFactor = 1.0)
        {
            MaxComputeTimePerAction = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);
            MaxComputeTimePerGame = TimeSpan.FromMilliseconds(1000 * timeScalingFactor);

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            _rnd = new ConsistantRandom(seed);
        }

        public void AddPlayer(ICamelCupPlayer playerInterface) 
        {
            var player = new Player(playerInterface, MaxComputeTimePerAction, MaxComputeTimePerGame);
            player.PerformAction(x => x.Load());
            player.PerformAction(x => player.Name = x.GetPlayerName());
            _players.Add(player);
        }

        public void Save()
        {
            foreach (var player in _players)
            {
                player.PerformAction(x => x.Save());
            }
        }

        public IEnumerable<Player> GetPlayers() 
        {
            return _players.ToList();
        }

        public CamelCupGame ComputeNewGame(int steps = -1)
        {
            return ComputeSeededGame(_rnd.Next(), _rnd.Next(), _rnd.Next(), steps);
        }
        
        public CamelCupGame ComputeSeededGame(int startPosSeed, int playerOrderSeed, int gameSeed, int steps = -1)
        {
            StartPositionSeed = startPosSeed;
            PlayerOrderSeed = playerOrderSeed;
            GameSeed = gameSeed;

            var startPos = GetRandomStartingPositions(startPosSeed);
            _players = GetRandomPlayerOrder(_players, playerOrderSeed);
            var game = new CamelCupGame(_players, startPos, gameSeed);

            game.StartGame();

            Step = 0;
            while (!game.IsComplete() && (steps < 0 || Step < steps))
            {
                game.MoveNextPlayer();
                Step++;
            }

            foreach (var winner in game.Winners())
            {
                winner.Wins++;
            }

            return game;
        }

        private Dictionary<CamelColor, Position> GetRandomStartingPositions(int seed) 
        {
            var rnd = new ConsistantRandom(seed);
            var i = 0;
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => new Position { Location = rnd.Next(0, 2), Height = i++ });
        }

        private List<Player> GetRandomPlayerOrder(List<Player> players, int seed)
        {
            var rnd = new ConsistantRandom(seed);

            // make original order not matter for determination
            players = players.OrderBy(x => x.Name).ToList();

            return players.OrderBy(x => rnd.Next()).ToList();
        }
    }
}
