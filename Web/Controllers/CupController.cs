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
        public static CupRunner Runner;

        public class RunnerState
        {
            public List<Player> Players;
            public Guid GameId;
        }

        [HttpGet("new")]
        public Guid New(string id = null)
        {
            if (Runner != null)
            {
                Runner.Stop();
            }

            Guid? guid = id != null ? (Guid?)Guid.Parse(id) : null;
            Runner = new CupRunner(guid);
            Runner.Load();
            return Runner.GameId;
        }

        [HttpGet]
        public RunnerState GetInfo()
        {
            if (Runner == null)
            {
                return null;
            }

            var runner = Runner.Runner;

            var result = new RunnerState()
            {
                Players = runner.GetPlayers().ToList(),
                GameId = Runner.GameId
            };

            return result;
        }

        [HttpGet("run")]
        public void Run()
        {
            if (Runner != null)
                Runner.Run();
        }
        
        [HttpGet("stop")]
        public void Stop()
        {
            if (Runner != null)
                Runner.Stop();
        }
    }
}
