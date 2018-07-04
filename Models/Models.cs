using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;

namespace Delver.CamelCup
{
    class Player 
    {
        private string _name;
        private int playerId;

        public string Name => _name ?? (_name = PerformAction(x => PlayerInterface.GetPlayerName()));

        public ICamelCupPlayer PlayerInterface { get; set; }

        public TimeSpan ComputationTime { get; set; }

        public int Money { get; set; }

        private TimeSpan MaxActionTime { get; set; }
        private TimeSpan MaxGameTime { get; set; }

        public Player(ICamelCupPlayer playerInterface, TimeSpan maxActionTime, TimeSpan maxGameTime)
        {
            PlayerInterface = playerInterface;
            MaxActionTime = maxActionTime;
            MaxGameTime = maxGameTime;
        }

        public void Reset(int playerId) 
        {
            this.playerId = playerId;
            ComputationTime = TimeSpan.FromMilliseconds(0);
            Money = 3;
        }

        public void PerformAction(Action<ICamelCupPlayer> action) 
        {
            var cts = new CancellationTokenSource(MaxActionTime);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try {
                var task = Task.Run(() => {
                    action.Invoke(PlayerInterface);
                }, cts.Token);
                
                task.Wait();
                watch.Stop();
            
                ComputationTime = ComputationTime.Add(watch.Elapsed);

                if (watch.Elapsed > MaxActionTime || ComputationTime > MaxGameTime)
                {
                    throw new PlayerLoseesException(playerId);
                }
            }
            catch {
                throw new PlayerLoseesException(playerId);
            }
        }

        public T PerformAction<T>(Func<ICamelCupPlayer, T> action) 
        {
            var cts = new CancellationTokenSource(MaxActionTime);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try {
                var task = Task.Run(() => {
                    return action.Invoke(PlayerInterface);
                }, cts.Token);

                task.Wait();
                watch.Stop();
            
                ComputationTime = ComputationTime.Add(watch.Elapsed);

                if (watch.Elapsed > MaxActionTime || ComputationTime > MaxGameTime)
                {
                    throw new PlayerLoseesException(playerId);
                }

                return task.Result;
            }
            catch {
                throw new PlayerLoseesException(playerId);
            }
        }
    }
}
