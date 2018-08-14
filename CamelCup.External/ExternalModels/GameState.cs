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

        public Dictionary<int, bool> Disqualified { get; set; } = new Dictionary<int, bool>();

        public List<BettingCard> BettingCards = new List<BettingCard>();

        public List<GameEndBet> WinnerBets = new List<GameEndBet>();

        public List<GameEndBet> LoserBets = new List<GameEndBet>();

        public List<CamelColor> RemainingDice = new List<CamelColor>();

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
