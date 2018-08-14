using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class NoActionStateChange : StateChange 
    {
        public NoActionStateChange(int player) : base(StateAction.NoAction, player, CamelColor.Blue, -1)
        {
        }
    }
}