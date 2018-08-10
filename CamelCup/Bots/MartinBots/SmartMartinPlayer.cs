using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class SmartMartinPlayer : ICamelCupPlayer
    {
        public string GetPlayerName()
        {
            return "Smart 5'er-Martin";
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
            var endStates = CamelHelper.GetAllGameEndStates(gameState, 2);

            var probability = CamelHelper.GetWinningProbability(endStates);
            var bettingCards = gameState.BettingCards
                .GroupBy(x => x.CamelColor)
                .ToDictionary(x => x.Key, y => y.Where(x => x.IsFree).Select(x => x.Value).DefaultIfEmpty(0).Max());

            var bets = probability.ToDictionary(x => x.Key, x => x.Value * bettingCards[x.Key]);
            var bestBet = bets.OrderByDescending(x => x.Value).First();

            if (bestBet.Value > 1.0) 
            {
                return new PlayerAction() { CamelAction = CamelAction.PickCard, Color = bestBet.Key };
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