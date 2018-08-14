namespace Delver.CamelCup.External
{
    public class PlayerAction 
    {
        public PlayerAction() : this(CamelAction.NoAction)
        {
        }

        public PlayerAction(CamelAction action, CamelColor color = CamelColor.Blue, int value = -1)
        {
            CamelAction = action;
            Color = color;
            Value = value;
        }

        public CamelAction CamelAction { get; set; }
        public int Value { get; set; }
        public CamelColor Color { get; set; }
    }
}
