using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class ImplementedPlayerAction : PlayerAction
    {
        public ImplementedPlayerAction()
        {
            CamelAction = CamelAction.NoAction;
        }

        public ImplementedPlayerAction(PlayerAction playerAction) 
        {
            CamelAction = playerAction.CamelAction;
            Color = playerAction.Color;
            Value = playerAction.Value;
        }

        public PlayerAction Clone() 
        {
            return new PlayerAction() 
            {
                CamelAction = CamelAction,
                Color = Color,
                Value = Value
            };
        }
    }
}
