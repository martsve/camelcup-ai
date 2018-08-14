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
    public class CamelBot
    {
        public string BotName;
        
        public Type BotType;

        public string FileName;
        public bool External;
        public Guid Id = Guid.NewGuid();
    }
}
