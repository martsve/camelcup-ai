using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Delver.CamelCup.External
{
    public class GameState 
    {
        public int Round { get; set; }

        public int BoardSize { get; set; }

        public List<Camel> Camels { get; set; } = new List<Camel>();

        public Dictionary<int, Trap> Traps { get; set; } = new Dictionary<int, Trap>();

        public Dictionary<int, int> Money { get; set; } = new Dictionary<int, int>();

        public List<BettingCard> BettingCards = new List<BettingCard>();

        public List<GameEndBet> WinnerBets = new List<GameEndBet>();

        public List<GameEndBet> LoserBets = new List<GameEndBet>();

        public List<CamelColor> RemainingDice = new List<CamelColor>();

        public GameState Clone()
        {
           return new GameState() 
            {
                Round = Round,
                BoardSize = BoardSize,
                Camels = Camels.Select(x => x.Clone()).ToList(),
                Traps = Traps.ToDictionary(x => x.Key, x => x.Value.Clone()),
                Money = Money.ToDictionary(x => x.Key, x => x.Value),
                RemainingDice = RemainingDice.ToList()
            };
        }

        public GameState Apply(StateChange change)
        {
            change.Apply(this);
            return this;
        }

        public GameState Apply(IEnumerable<StateChange> changes)
        {
            foreach (var change in changes)
                change.Apply(this);
                
            return this;
        }

        public GameState Revert(StateChange change)
        {
            change.Revert(this);
            return this;
        }
    }
}
