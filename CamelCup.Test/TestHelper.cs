using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.Collections.Generic;

namespace CamelCup.Test
{
    public static class TestHelper
    {
        public static Dictionary<CamelColor, Position> ConvertToStartingPositions(params int[] startingPositions) 
        {
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => new Position { Location = startingPositions[(int)x], Height = (int)x });
        }
        
        public static Position GetPosition(this GameState gamestate, CamelColor color)
        {
            var camel = gamestate.Camels.First(x => x.CamelColor == color);
            return new Position { Location = camel.Location, Height = camel.Height };
        }
    }
}
