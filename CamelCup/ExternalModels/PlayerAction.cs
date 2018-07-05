using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup.External
{
    public class PlayerAction 
    {
        public CamelAction CamelAction { get; set; }
        public int Value { get; set; }
        public CamelColor Color { get; set; }
    }
}
