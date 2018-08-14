using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class MartinPlayer : ICamelCupPlayer
    {
        public string GetPlayerName()
        {
            return "5'er-Martin";
        }

        public void StartNewGame(int playerId, GameInfo info, GameState gameState)
        {
        }

        public void InformAboutAction(int player, PlayerAction action, GameState gameState)
        {
        }

        public void Winners(List<int> winners, GameState gameState)
        {
        }

        public PlayerAction GetAction(GameState gameState)
        {
            var leaders = gameState.GetLeadingOrder();
            var bestBets = gameState.BettingCards.Where(x => x.IsFree).OrderByDescending(x => x.Value).ThenBy(x => leaders.IndexOf(x.CamelColor)).ToList();
            var bestBet = bestBets.FirstOrDefault();

            if (bestBet != null && bestBet.Value > 2) 
            {
                return new PlayerAction() { CamelAction = CamelAction.PickCard, Color = bestBet.CamelColor };
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