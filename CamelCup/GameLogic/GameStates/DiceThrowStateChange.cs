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
        private List<CamelColor> movedStack;
        private int oldLocation;
        private int trapPlayer = -1;

        public DiceThrowStateChange(int player, CamelColor color, int value) : base(StateAction.ThrowDice, player, color, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            gameState.Money[Player] += 1;
            gameState.RemainingDice.Remove(Color);

            var mainCamel = gameState.Camels.First(x => x.CamelColor == Color);
            var camelStack = gameState.Camels.Where(x => x.Location == mainCamel.Location && x.Height >= mainCamel.Height).ToList();
            movedStack = camelStack.Select(x => x.CamelColor).ToList();

            oldLocation = mainCamel.Location;

            var newLocation = oldLocation + Value;
            var onTop = true;
            var trap = gameState.Traps.FirstOrDefault(x => x.Value.Location == newLocation);
            if (trap.Value != null) 
            {
                trapPlayer = trap.Key;
                gameState.Money[trapPlayer] += 1;
                newLocation += trap.Value.Move;
                onTop = trap.Value.Move > 0;
            }

            MoveStack(gameState.Camels, camelStack, newLocation, onTop);
        }

        private List<Camel> MoveStack(List<Camel> camels, List<Camel> movingCamels, int location, bool onTop)
        {
            foreach (var camel in movingCamels)
            {
                camel.Location = location;
                camel.Height += 500 * (onTop ? 1 : -1);
            }

            var height = 0;
            foreach (var camel in camels.OrderBy(x => x.Location).ThenBy(x => x.Height))
            {
                camel.Height = height++;
            }

            return camels;
        }

        public override void Reverse(GameState gameState) 
        {
            gameState.Money[Player] -= 1;

            gameState.RemainingDice.Add(Color);

            if (trapPlayer >= 0)
            {
                gameState.Money[trapPlayer] -= 1;
            }

            var movedCamels = movedStack.Select(x => gameState.Camels.First(y => y.CamelColor == x)).ToList();

            MoveStack(gameState.Camels, movedCamels, oldLocation, true);
        }
    }
}