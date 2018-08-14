using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PickCardStateChange : StateChange 
    {
        public PickCardStateChange(int player, CamelColor color, int value) : base(StateAction.PickCard, player, color, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            var card = gameState.BettingCards.First(x => x.IsFree && x.CamelColor == Color && x.Value == Value);
            card.Owner = Player;
        }

        public override void Revert(GameState gameState) 
        {
            var card = gameState.BettingCards.First(x => x.Value == Value && x.CamelColor == Color);
            card.Owner = -1;
        }
    }
}