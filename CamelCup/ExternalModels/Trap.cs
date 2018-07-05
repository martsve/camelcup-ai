using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup.External
{
    public class Trap
    {
        public int Move { get; set; }
        public int Location { get; set; } = -1;

        public Trap Clone()
        {
            return new Trap() { Move = Move, Location = Location };
        }
    }
}
