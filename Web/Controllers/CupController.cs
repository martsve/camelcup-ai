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

        [HttpGet("new")]
        public Guid New(string id = null)
        {
            Guid? guid = id != null ? (Guid?)Guid.Parse(id) : null;
            Runner = new CupRunner(guid);
            return Runner.GameId;
        }

        [HttpGet("run")]
        public GameState Run()
        {
            return Runner.Run();
        }
    }
}
