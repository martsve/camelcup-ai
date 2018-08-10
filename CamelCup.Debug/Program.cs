using System;
using Delver.CamelCup;
using Delver.CamelCup.MartinBots;
using System.Linq;

using CamelCup.Test;
using Delver.CamelCup.External;

namespace CamelCup.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1: blue | 2: green, orange | 3: red, yellow
            var startingPositions = TestHelper.ConvertToStartingPositions("1,0 2,1 2,2 3,3 3,4");
            var gamestate = new ImplementedGameState(2, startingPositions);

            var engine = new RulesEngine(gamestate, seed: 2);
            var change = engine.Getchange(0, new PlayerAction() { CamelAction = CamelAction.PickCard, Color = CamelColor.Red }) as PickCardStateChange;

        }
    }
}
