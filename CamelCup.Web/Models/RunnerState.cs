using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Delver.CamelCup.Web.Services;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;

namespace Delver.CamelCup.Web.Models
{       
    public class RunnerState
    {
        public List<Player> Players;
        public Guid CupId;
    }
}
