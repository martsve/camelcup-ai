using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace CamelCup.Web
{
    public class CupRunner
    {
        public CamelRunner Runner { get; set; }

        public Guid GameId { get; set; }

        private Task GameTask { get; set; }
        private CancellationTokenSource cts;

        public CupRunner(Guid? gameId = null)
        {
            GameId = gameId ?? Guid.NewGuid();
            var seed = GameId.GetHashCode();
            Runner = new CamelRunner(seed: seed);
        }

        public void Load()
        {
            Runner.AddPlayer(new DiceThrower());
            Runner.AddPlayer(new RandomBot(1, seed: 1));
            Runner.AddPlayer(new MartinPlayer());
            Runner.AddPlayer(new SmartMartinPlayer());
        }

        public void Run()
        {
            Stop();
            cts = new CancellationTokenSource();
            GameTask = Task.Run(() => ComputeGame(cts.Token));
        }

        public void Stop()
        {
            if (GameTask != null && GameTask.Status == TaskStatus.Running)
            {
                cts.Cancel();
                while (!GameTask.IsCompleted) {
                    Thread.Sleep(100);
                }
            }
        }

        private void ComputeGame(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                Runner.ComputeNewGame();
            }
        }
    }
}
