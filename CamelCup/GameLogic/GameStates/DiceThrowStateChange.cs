using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class DiceThrowStateChange : StateChange 
    {
        public DiceThrowStateChange(int player, CamelColor color, int value) : base(StateAction.ThrowDice, player, color, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            gameState.Money[Player] += 1;

            gameState.RemainingDice.Remove(Color);

            var mainCamel = gameState.Camels.First(x => x.CamelColor == Color);
            var camelStack = gameState.Camels.Where(x => x.Location == mainCamel.Location && x.Height >= mainCamel.Height).ToList();
            camelStack.ForEach(x => x.Height += 500);

            var oldLocation = mainCamel.Location;
            var newLocation = mainCamel.Location + Value;

            var trap = gameState.Traps.FirstOrDefault(x => x.Value.Location == newLocation);
            if (trap.Value != null) 
            {
                gameState.Money[trap.Key] += 1;
                newLocation += trap.Value.Move;

                if (trap.Value.Move == -1) 
                {
                    camelStack.ForEach(x => x.Height -= 1000);
                }
            }

            camelStack.ForEach(x => x.Location = newLocation);

            foreach (var group in gameState.Camels.GroupBy(x => x.Location))
            {
                var height = 0;
                foreach (var camel in group.OrderBy(x => x.Height).ToList())
                {
                    camel.Height = height;
                    height++;
                }
            }
        }

        public override void Reverse(GameState state) 
        {

        }
    }
}