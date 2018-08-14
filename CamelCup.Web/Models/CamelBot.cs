using System;

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
