using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class WinnerBetStateChange : StateChange 
    {
        public WinnerBetStateChange(int player, CamelColor color) : base(StateAction.SecretBetOnWinner, player, color, -1)
        {
        }

        public override void Apply(GameState gameState)
        {
            gameState.WinnerBets.Add(new GameEndBet() { Player = Player, CamelColor = Color });
        }

        public override void Revert(GameState gameState) 
        {
            gameState.WinnerBets.RemoveAll(x => x.Player == Player && x.CamelColor == Color);
        }
    }
}