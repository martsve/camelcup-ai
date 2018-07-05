using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup
{
    class PlayerLoseesException : Exception 
    {
        public int PlayerId { get; set; }

        public PlayerLoseesException(int playerId) : base()
        {
            PlayerId = playerId;
        }         
    }
}
