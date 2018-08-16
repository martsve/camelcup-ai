namespace Delver.CamelCup.External
{
    public class GameEndBet 
    {
        public GameEndBet() 
        {}

        public GameEndBet(int player, CamelColor color)
        {
            Player = player;
            CamelColor = color;
        }

        public int Player { get; set; }

        public CamelColor? CamelColor { get; set; }
    }
}
