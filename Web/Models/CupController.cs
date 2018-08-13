using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CamelCup.Web;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;

namespace Web.Models
{       
     public class GameResult 
    {
        public int? StartPositionSeed;
        public int? GameSeed;
        public int? PlayerOrderSeed;

        public Guid GameId;
        public Dictionary<int, string> Players;
        public List<int> Winners;
        public GameState EndState;
        public List<StateChange> History;
    }
}
