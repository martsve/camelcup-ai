using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Delver.CamelCup
{
    public class RulesEngine 
    {
       public bool Validate(GameState gameState, int playerId, PlayerAction action) 
       {
           return true;
       }

       public void Iterate(GameState gameState, int playerId, PlayerAction action) 
       {
           
       }
    }
}