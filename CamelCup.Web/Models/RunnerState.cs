using System;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Models
{       
    public class RunnerState
    {
        public List<Player> Players;
        public Guid CupId;
        public int TotalGames;
        public int GamesPlayed;
        public Player Winner;
    }
}
