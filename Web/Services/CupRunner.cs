using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System;

namespace CamelCup.Web
{
    public class CupRunner
    {
        public CamelRunner Runner { get; set; }

        public Guid GameId { get; set; }

        public CupRunner(Guid? gameId = null)
        {
            GameId = gameId ?? Guid.NewGuid();
            var seed = GameId.GetHashCode();
            Runner = new CamelRunner(seed: seed);
        }

        public GameState Run()
        {
            Runner.AddPlayer(new RandomBot(1, seed: 2));
            Runner.AddPlayer(new RandomBot(2, seed: 3));
            var game = Runner.ComputeNewGame();
            return game.GameState;
        }
    }
}
