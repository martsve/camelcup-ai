using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class NoActionStateChange : StateChange 
    {
        public NoActionStateChange(int player) : base(StateAction.NoAction, player, CamelColor.Blue, -1)
        {
        }
    }
}