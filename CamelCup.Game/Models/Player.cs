using System;
using System.Threading;
using System.Threading.Tasks;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class Player 
    {
        public int PlayerId;

        public string Name { get; set; }

        public bool Disqualified = false;

        public int Wins { get; set; }

        public ICamelCupPlayer PlayerInterface { get; set; }

        public TimeSpan ComputationTime { get; set; }

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
            PlayerId = playerId;
            Disqualified = false;
            ComputationTime = TimeSpan.FromMilliseconds(0);
        }

        public void PerformAction(Action<ICamelCupPlayer> action) 
        {
            if (Disqualified)
                return;

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
                    throw new PlayerLoseesException(PlayerId);
                }
            }
            catch {
                throw new PlayerLoseesException(PlayerId);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
