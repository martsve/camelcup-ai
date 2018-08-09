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
        public PlussTrapStateChange(int player, int value) : base(StateAction.PlacePlussTrap, player, CamelColor.Blue, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            gameState.Traps[Player].Location = Value;
            gameState.Traps[Player].Move = 1;
        }

        public override void Reverse(GameState state) 
        {
        }
    }
}