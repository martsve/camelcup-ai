using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Delver.CamelCup.Web.Models;
using Newtonsoft.Json;

namespace Delver.CamelCup.Web.Services
{
    public class CamelService
    {
        public CamelRunner Runner { get; set; }

        public Guid CupId { get; set; }

        public List<Guid> GameIdHistory { get; set; }

        private Task GameTask { get; set; }
        private CancellationTokenSource _cts;

        public CamelService(Guid? cupId = null)
        {
            CupId = cupId ?? Guid.NewGuid();
            var seed = CupId.GetHashCode();
            Runner = new CamelRunner(seed: seed);
            GameIdHistory = new List<Guid>();
        }

        public void Load()
        {
            Runner.AddPlayer(new DiceThrower());
            Runner.AddPlayer(new RandomBot(1, seed: 1));
            Runner.AddPlayer(new MartinPlayer());
            Runner.AddPlayer(new SmartMartinPlayer());
        }

        public void Load(ICamelCupPlayer player)
        {
            Runner.AddPlayer(player);
        }

        public void Run()
        {
            Stop();
            _cts = new CancellationTokenSource();
            GameTask = Task.Run(() => ComputeGame(_cts.Token));
        }

        public void Stop()
        {
            if (GameTask != null && GameTask.Status == TaskStatus.Running)
            {
                _cts.Cancel();
                while (!GameTask.IsCompleted) {
                    Thread.Sleep(100);
                }
            }
        }
        
        public GameResult GetGame()
        {
            var game = Runner.ComputeNewGame();
            GameIdHistory.Add(game.GameId);
        
            return new GameResult()
            {
                 RunnerSeed = CupId.GetHashCode(),
                 StartPositionSeed = Runner.StartPositionSeed,
                 GameSeed = Runner.GameSeed,
                 PlayerOrderSeed = Runner.PlayerOrderSeed,
                 History = game.History,
                 GameId = game.GameId,
                 EndState = game.GameState,
                 Players = Runner.GetPlayers().ToDictionary(x => x.PlayerId, x => x.Name),
                 Winners = game.Winners().Select(x => x.PlayerId).ToList()
            };
        }

        private void ComputeGame(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var game = Runner.ComputeNewGame();
                GameIdHistory.Add(game.GameId);
            }
        }
    }
}