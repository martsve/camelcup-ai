using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class MinusTrapStateChange : StateChange 
    {
        private int oldValue;
        private int oldMove;
        
        public MinusTrapStateChange(int player, int value) : base(StateAction.PlaceMinusTrap, player, CamelColor.Blue, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            oldValue = gameState.Traps[Player].Location;
            oldMove = gameState.Traps[Player].Move;

            gameState.Traps[Player].Location = Value;
            gameState.Traps[Player].Move = -1;
        }

        public override void Reverse(GameState gameState) 
        {
            gameState.Traps[Player].Location = oldValue;
            gameState.Traps[Player].Move = oldMove;
        }
    }
}