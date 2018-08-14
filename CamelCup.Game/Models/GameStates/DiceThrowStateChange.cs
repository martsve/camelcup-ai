using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class DiceThrowStateChange : StateChange 
    {
        private List<CamelColor> _movedStack;
        private int _oldLocation;
        private int _trapPlayer = -1;

        public DiceThrowStateChange(int player, CamelColor color, int value) : base(StateAction.ThrowDice, player, color, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            ChildChanges = new List<StateChange>();

            gameState.RemainingDice.Remove(Color);

            gameState.Money[Player] += 1;
            ChildChanges.Add(new StateChange(StateAction.GetMoney, Player, Color, gameState.Money[Player]));

            var mainCamel = gameState.Camels.First(x => x.CamelColor == Color);
            var camelStack = gameState.Camels.Where(x => x.Location == mainCamel.Location && x.Height >= mainCamel.Height).ToList();
            _movedStack = camelStack.OrderBy(x => x.Height).Select(x => x.CamelColor).ToList();

            _oldLocation = mainCamel.Location;

            var newLocation = _oldLocation + Value;
            var onTop = true;
            var trap = gameState.Traps.FirstOrDefault(x => x.Value.Location == newLocation);
            if (trap.Value != null) 
            {
                _trapPlayer = trap.Key;
                gameState.Money[_trapPlayer] += 1;
                ChildChanges.Add(new MoveStackStateChange(_movedStack, _oldLocation, newLocation, onTop));
                ChildChanges.Add(new StateChange(StateAction.GetMoney, _trapPlayer, Color, gameState.Money[_trapPlayer]));
                newLocation += trap.Value.Move;
                onTop = trap.Value.Move > 0;
            }

            ChildChanges.Add(new MoveStackStateChange(_movedStack, _oldLocation, newLocation, onTop));

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

        public override void Revert(GameState gameState) 
        {
            gameState.Money[Player] -= 1;

            gameState.RemainingDice.Add(Color);

            if (_trapPlayer >= 0)
            {
                gameState.Money[_trapPlayer] -= 1;
            }

            var movedCamels = _movedStack.Select(x => gameState.Camels.First(y => y.CamelColor == x)).ToList();

            MoveStack(gameState.Camels, movedCamels, _oldLocation, true);
        }
    }
}