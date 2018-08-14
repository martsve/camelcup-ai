using Microsoft.VisualStudio.TestTools.UnitTesting;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;
using Delver.CamelCup.External;
using System.Collections.Generic;

namespace Delver.CamelCup.Web.Services.Test
{
    public static class TestHelper
    {
        public static Dictionary<CamelColor, Position> ConvertToStartingPositions(string startingPositions) 
        {
            var positions = startingPositions.Split(' ').Select(x => new Position(int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1]))).ToArray();
            return CamelHelper.GetAllCamelColors().ToDictionary(x => x, x => positions[(int)x]);
        }
        
        public static Position GetPosition(this GameState gamestate, CamelColor color)
        {
            var camel = gamestate.Camels.First(x => x.CamelColor == color);
            return new Position { Location = camel.Location, Height = camel.Height };
        }

        public static string CamelPositionToString(this GameState state)
        {
            return string.Join(" ", state.Camels.Select(x => new Position(x.Location, x.Height)));
        }
    }
}
