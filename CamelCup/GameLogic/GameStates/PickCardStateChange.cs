using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PickCardStateChange : StateChange 
    {
        public PickCardStateChange(int player, CamelColor color) : base(StateAction.PickCard, player, color, -1)
        {
        }

        public override void Apply(GameState gameState)
        {
            var card = gameState.BettingCards.Where(x => x.IsFree && x.CamelColor == Color).OrderByDescending(x => x.Value).First();
            card.Owner = Player;
        }

        public override void Reverse(GameState gameState) 
        {
            var card = gameState.BettingCards.Where(x => x.IsFree && x.CamelColor == Color).OrderByDescending(x => x.Value).First();
            card.Owner = -1;
        }
    }
}