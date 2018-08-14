using System;

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
