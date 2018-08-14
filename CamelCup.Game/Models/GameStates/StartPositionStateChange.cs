using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class StartPositionStateChange : StateChange 
    {
        public int Height;
        public StartPositionStateChange(Camel camel) : base(StateAction.StartPosition, -1, camel.CamelColor, camel.Location)
        {
            Height = camel.Height;
        }
    }
}