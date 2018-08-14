using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PlussTrapStateChange : StateChange 
    {
        private int oldValue;
        private int oldMove;

        public PlussTrapStateChange(int player, int value) : base(StateAction.PlacePlussTrap, player, CamelColor.Blue, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            oldValue = gameState.Traps[Player].Location;
            oldMove = gameState.Traps[Player].Move;

            gameState.Traps[Player].Location = Value;
            gameState.Traps[Player].Move = 1;
        }

        public override void Revert(GameState gameState) 
        {
            gameState.Traps[Player].Location = oldValue;
            gameState.Traps[Player].Move = oldMove;
        }
    }
}