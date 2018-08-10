using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class RulesEngine 
    {
        private Random rnd { get; set; }

        public RulesEngine(int seed = -1)
        {
            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            rnd = new Random(seed);
        }

        public StateChange Getchange(GameState gameState, int playerId, PlayerAction action) 
        {
            if (action.CamelAction == CamelAction.ThrowDice)
            {
                if (!gameState.RemainingDice.Any())
                {
                    return null;
                }

                var index = rnd.Next(0, gameState.RemainingDice.Count);
                var color = gameState.RemainingDice[index];
                var value = rnd.Next(1, 4);

                return new DiceThrowStateChange(playerId, color, value);    
            }
            else if (action.CamelAction == CamelAction.PickCard)
            { 
                var bet = gameState.BettingCards.Where(x => x.IsFree && x.CamelColor == action.Color).FirstOrDefault();

                if (bet != null) {
                    return new PickCardStateChange(playerId, action.Color);
                }
                
                return null;
            }
            else if (action.CamelAction == CamelAction.PlaceMinusTrap)
            {
                if (CamelHelper.IsValidTrapSpace(gameState, playerId, action.Value, positive: false))
                {
                    return new MinusTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.PlacePlussTrap)
            { 
                if (CamelHelper.IsValidTrapSpace(gameState, playerId, action.Value, positive: true))
                {
                    return new PlussTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.SecretBetOnLoser || action.CamelAction == CamelAction.SecretBetOnWinner)
            { 
                if (gameState.WinnerBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return null;

                if (gameState.LoserBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return null;

                if (action.CamelAction == CamelAction.SecretBetOnLoser)
                    return new LoserBetStateChange(playerId, action.Color);
                else 
                    return new WinnerBetStateChange(playerId, action.Color);
            }
            
            return new NoActionStateChange(playerId);
        }

        public void ScoreRound(GameState state)
        {
            var order = state.GetLeadingOrder();
            var first = order.First();
            var second = order.Skip(1).First();

            foreach (var bet in state.BettingCards.Where(x => !x.IsFree))
            {
                if (bet.CamelColor == first)
                {
                    state.Money[bet.Owner] += bet.Value;
                }
                else if (bet.CamelColor == second)
                {
                    state.Money[bet.Owner] += 1;
                }
                else
                {
                    if (state.Money[bet.Owner] > 0)
                        state.Money[bet.Owner] -= 1;
                }
            }
        }

        public void ScoreGame(GameState state)
        {
            var winner = state.GetLeadingOrder().First();
            var scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in state.WinnerBets)
            {
                if (card.CamelColor == winner) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    state.Money[card.Player] += score;
                }
                else
                {
                    if (state.Money[card.Player] > 0)
                        state.Money[card.Player] -= 1;
                }
            }

            var loser = state.GetLeadingOrder().Last();
            scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in state.LoserBets)
            {
                if (card.CamelColor == loser) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    state.Money[card.Player] += score;
                }
                else
                {
                    if (state.Money[card.Player] > 0)
                        state.Money[card.Player] -= 1;
                }
            }

        }
        
        public List<int> GetWinners(GameState state)
        {
            var max = state.Money.OrderByDescending(x => x.Value).First().Value;
            return state.Money.Where(x => x.Value == max).Select(x => x.Key).ToList();
        }
    }
}