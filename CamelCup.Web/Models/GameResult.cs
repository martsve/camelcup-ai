using System;
using System.Collections.Generic;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Models
{       
     public class GameResult 
    {
        public int? StartPositionSeed;
        public int? GameSeed;
        public int? PlayerOrderSeed;
        public int? RunnerSeed;

        public Guid GameId;
        public Dictionary<int, string> Players;
        public List<int> Winners;
        public GameState EndState;
        public List<StateChange> History;
    }
}
