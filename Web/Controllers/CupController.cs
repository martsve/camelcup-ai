using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CamelCup.Web;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class CupController : Controller
    {
        public static CamelService CamelService;

        public class RunnerState
        {
            public List<Player> Players;
            public Guid CupId;
        }

        [HttpGet("new")]
        public Guid New(string id = null)
        {
            CamelService?.Stop();

            Guid? guid = id != null ? (Guid?)Guid.Parse(id) : null;
            CamelService = new CamelService(guid);
            CamelService.Load();
            return CamelService.CupId;
        }

        [HttpGet]
        public RunnerState GetInfo()
        {
            if (CamelService == null)
            {
                return null;
            }

            var runner = CamelService.Runner;

            var result = new RunnerState()
            {
                Players = runner.GetPlayers().ToList(),
                CupId = CamelService.CupId
            };

            return result;
        }       

        [HttpGet("history")]
        public List<Guid> History()
        {
            return CamelService?.GameIdHistory;
        }

        [HttpGet("run")]
        public void Run()
        {
            CamelService?.Run();
        }
        
        [HttpGet("stop")]
        public void Stop()
        {
            CamelService?.Stop();
        }
    }
}
