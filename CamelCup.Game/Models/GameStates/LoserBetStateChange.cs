using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class LoserBetStateChange : StateChange 
    {
        public LoserBetStateChange(int player, CamelColor color) : base(StateAction.SecretBetOnLoser, player, color, -1)
        {
        }

        public override void Apply(GameState gameState)
        {
            gameState.LoserBets.Add(new GameEndBet() { Player = Player, CamelColor = Color });
        }

        public override void Revert(GameState gameState) 
        {
            gameState.LoserBets.RemoveAll(x => x.Player == Player && x.CamelColor == Color);
        }
    }
}