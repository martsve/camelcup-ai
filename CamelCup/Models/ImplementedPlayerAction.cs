using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class ImplementedPlayerAction : PlayerAction
    {
        public ImplementedPlayerAction()
        {
            CamelAction = CamelAction.NoAction;
        }

        public ImplementedPlayerAction(PlayerAction playerAction) 
        {
            CamelAction = playerAction.CamelAction;
            Color = playerAction.Color;
            Value = playerAction.Value;
        }

        public PlayerAction Clone() 
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PlayerAction>(serialized);
        }
    }
}
