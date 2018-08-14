using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PlussTrapStateChange : StateChange 
    {
        private int _oldValue;
        private int _oldMove;

        public PlussTrapStateChange(int player, int value) : base(StateAction.PlacePlussTrap, player, CamelColor.Blue, value)
        {
        }

        public override void Apply(GameState gameState)
        {
            _oldValue = gameState.Traps[Player].Location;
            _oldMove = gameState.Traps[Player].Move;

            gameState.Traps[Player].Location = Value;
            gameState.Traps[Player].Move = 1;
        }

        public override void Revert(GameState gameState) 
        {
            gameState.Traps[Player].Location = _oldValue;
            gameState.Traps[Player].Move = _oldMove;
        }
    }
}