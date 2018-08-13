﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CamelCup.Web;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using Delver.CamelCup.External;
using Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class CupController : Controller
    {
        public static CamelService CamelService;
        public static string BotPath = @"E:\Temp\bots";

        public static List<CamelBot> CamelBots;

        public class CamelBot
        {
            public string BotName;
            
            public Type BotType;

            public string FileName;
            public bool External;
            public Guid Id = Guid.NewGuid();
        }

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
            return CamelService.CupId;
        }

        [HttpGet("add/{id}")]
        public void Add(Guid id)
        {
            var bot = CamelBots.FirstOrDefault(x => x.Id == id);
            if (bot != null && CamelService != null)
            {
                var instance = Activator.CreateInstance(bot.BotType) as ICamelCupPlayer;
                CamelService.Load(instance);
            }
        }

        [HttpGet("bots")]
        public List<CamelBot> Bots()
        {
            if (CamelBots == null)
            {
                CamelBots = new List<CamelBot>()
                {
                    CreateCamelBot(typeof(DiceThrower)),
                    CreateCamelBot(typeof(HeatmapMinus)),
                    CreateCamelBot(typeof(HeatmapPluss)),
                    CreateCamelBot(typeof(IllegalBot)),
                    CreateCamelBot(typeof(MartinPlayer)),
                    CreateCamelBot(typeof(RandomBot)),
                    CreateCamelBot(typeof(SmartMartinPlayer)),
                    CreateCamelBot(typeof(SmartPluss)),
                };
    
                var externalFiles = Directory.GetFiles(BotPath);
                foreach (var file in externalFiles)
                {
                    var bot = GetExternalBot(file);
                    if (bot != null)
                        CamelBots.Add(bot);
                }
            }

            var newFiles = Directory.GetFiles(BotPath).Where(x => !CamelBots.Any(y => y.External == true && y.FileName == x));
            foreach (var file in newFiles)
            {
                CamelBots.Add(GetExternalBot(file));
            }

            return CamelBots;
        }       

        private CamelBot CreateCamelBot(Type type)
        {
            try {
                var impl = Activator.CreateInstance(type) as ICamelCupPlayer;
                var playerName = impl.GetPlayerName();

                var bot = new CamelBot() { 
                    BotType = type,
                    BotName = playerName
                };

                return bot;
            }
            catch (Exception) {
                return null;
            }
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
        
        [HttpGet("single")]
        public GameResult Single()
        {
            return CamelService?.GetGame();
        }
        
        [HttpGet("stop")]
        public void Stop()
        {
            CamelService?.Stop();
        }

        [HttpPost("upload")]
        public int Upload()
        {
            var form = Request.Form;
            var formFiles = Request.Form.Files;
            var uploaded = 0;
            foreach (var file in formFiles)
            {
                var filename = file.FileName;
                if (filename.EndsWith(".dll"))
                {
                    var localFile = Path.GetTempFileName();
                    using (var stream = new FileStream(localFile, FileMode.Truncate))
                    {
                        file.CopyTo(stream);
                    }

                    var botname =  GetBotName(localFile);
                    if (botname != null) 
                    {
                        filename = GetValidFileName(botname + "." + filename);
                        var destination = Path.Combine(BotPath, filename);
                        System.IO.File.Copy(localFile, destination, true);
                        uploaded++;
                    }
                }
            }
            
            return uploaded;
        }

        private static string GetValidFileName(string fileName) 
        {
            // remove any invalid character from the filename.
            String ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", String.Empty);
        }

        private string GetBotName(string filename)
        {
            return GetExternalBot(filename)?.BotName;
        }
        
        private CamelBot GetExternalBot(string filename)
        {
            try {
                var lib = AssemblyLoadContext.Default.LoadFromAssemblyPath(filename);
                foreach (Type type in lib.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(ICamelCupPlayer)))
                    {
                        var bot = Activator.CreateInstance(type) as ICamelCupPlayer;
                        var name = bot.GetPlayerName();
                        return new CamelBot()
                        {
                            BotName = name,
                            FileName = filename,
                            External = true,
                            BotType = type
                        };
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}