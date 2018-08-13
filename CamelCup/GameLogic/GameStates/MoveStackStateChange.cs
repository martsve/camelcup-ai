using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class MoveStackStateChange : StateChange 
    {
        public List<CamelColor> Stack { get; set; }
        public int From;
        public int To;
        public bool LandOnTop;

        public MoveStackStateChange(List<CamelColor> stack, int from, int to, bool onTop) : base(StateAction.Move, -1, CamelColor.Blue, -1)
        {
            Stack = stack;
            From = from;
            To = to;
            LandOnTop = onTop;
        }
    }
}