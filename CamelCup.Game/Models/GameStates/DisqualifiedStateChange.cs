using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class DisqualifiedStateChange : StateChange 
    {
        public PlayerAction PlayerAction { get; set; }

        public DisqualifiedStateChange(int player, PlayerAction action) : base(StateAction.Disqualified, player, CamelColor.Blue, -1)
        {
            PlayerAction = action;
        }
    }
}