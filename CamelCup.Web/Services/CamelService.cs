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

        public int TotalGames { get; set; }
        public int GamesPlayed { get; set; }

        public List<Guid> GameIdHistory { get; set; }
        
        public GameResult LastGameResult { get; set; }

        private Task GameTask { get; set; }
        private CancellationTokenSource _cts;
        private object LockObject = new Object();

        public CamelService(Guid? cupId = null, bool ignoreTime = false, int totalGames = 1000)
        {
            TotalGames = totalGames;
            CupId = cupId ?? Guid.NewGuid();
            var seed = CupId.GetHashCode();
            Runner = new CamelRunner(seed: seed, timeScalingFactor: ignoreTime ? 1000 : 1);
            GameIdHistory = new List<Guid>();
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

        public GameResult GetGame(int? startPosSeed, int? playerSeed, int? gameSeed)
        {
            CamelCupGame game;
            if (startPosSeed.HasValue && playerSeed.HasValue && gameSeed.HasValue) 
            {
                game = Runner.ComputeSeededGame(startPosSeed.Value, playerSeed.Value, gameSeed.Value);
            }
            else {
                game = Runner.ComputeNewGame();
            }
            GameIdHistory.Add(game.GameId);
            SetLastGame(game);
            return LastGameResult;
        }
        
        private GameResult CamelCupGameToResult(CamelCupGame game)
        {
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
                SetLastGame(game);
                GameIdHistory.Add(game.GameId);
                GamesPlayed++;

                if (GamesHasWinner())
                {
                    break;
                }
            }
        }

        private Player[] PlayerOrder => Runner.GetPlayers().OrderByDescending(x => x.Wins).ToArray();

        public Player GetWinner()
        {
            if (GameTask != null && GameTask.Status == TaskStatus.Running)
            {
                return null;
            }

            return PlayerOrder.FirstOrDefault();
        }

        private bool GamesHasWinner() 
        {
            var leaders = PlayerOrder.ToArray();
            if (leaders.Count(x => x.Wins == PlayerOrder[0].Wins) > 1)
            {
                return false;
            }

            if (GamesPlayed >= TotalGames)
            {
                return true;
            }

            var remaining = TotalGames - GamesPlayed;

            if (leaders.Count(x => x.Wins + remaining > PlayerOrder[0].Wins) == 1)
            {
                return true;
            }
                
            return false;
        }

        private void SetLastGame(CamelCupGame game)
        {
            lock (LockObject)
            {
                LastGameResult = CamelCupGameToResult(game);
            }
        }
    }
}
