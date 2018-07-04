using System;
using System.Linq;
using Delver.CamelCup;

namespace Delver.CamelCup.MartinBots
{
    public class MartinPlayer : ICamelCupPlayer
    {
        public string GetPlayerName()
        {
            return "Martin The Better";
        }

        public void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState)
        {
        }

        public void InformAboutAction(int player, PlayerAction action, GameState gameState)
        {
        }

        public PlayerAction GetAction(GameState gameState)
        {
            var leaders = gameState.GetLeadingOrder();
            var bestBets = gameState.BettingCards.Free().OrderByDescending(x => x.Value).ThenBy(x => leaders.IndexOf(x.CamelColor));
            var bestBet = bestBets.FirstOrDefault();

            if (bestBet != null && bestBet.Value > 2) 
            {
                return new PlayerAction() { CamelAction = CamelAction.PickCard, CamelColor = bestBet.CamelColor };
            }

            return new PlayerAction() { CamelAction = CamelAction.ThrowDice };
        }

        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}