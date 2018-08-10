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

        private GameState gamestate { get; set; }

        public RulesEngine(GameState gamestate, int seed = -1)
        {
            this.gamestate = gamestate;

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            rnd = new Random(seed);
        }

        public StateChange Getchange(int playerId, PlayerAction action) 
        {
            if (action.CamelAction == CamelAction.ThrowDice)
            {
                if (!gamestate.RemainingDice.Any())
                {
                    return null;
                }

                var index = rnd.Next(0, gamestate.RemainingDice.Count);
                var color = gamestate.RemainingDice[index];
                var value = rnd.Next(1, 4);

                return new DiceThrowStateChange(playerId, color, value);    
            }
            else if (action.CamelAction == CamelAction.PickCard)
            { 
                if (gamestate.BettingCards.Any(x => x.IsFree && x.CamelColor == action.Color)) {
                    var card = gamestate.BettingCards.Where(x => x.IsFree && x.CamelColor == action.Color).OrderByDescending(x => x.Value).First();
                    return new PickCardStateChange(playerId, action.Color, card.Value);
                }
                
                return null;
            }
            else if (action.CamelAction == CamelAction.PlaceMinusTrap)
            {
                if (CamelHelper.IsValidTrapSpace(gamestate, playerId, action.Value, positive: false))
                {
                    return new MinusTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.PlacePlussTrap)
            { 
                if (CamelHelper.IsValidTrapSpace(gamestate, playerId, action.Value, positive: true))
                {
                    return new PlussTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.SecretBetOnLoser || action.CamelAction == CamelAction.SecretBetOnWinner)
            { 
                if (gamestate.WinnerBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return null;

                if (gamestate.LoserBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return null;

                if (action.CamelAction == CamelAction.SecretBetOnLoser)
                    return new LoserBetStateChange(playerId, action.Color);
                else 
                    return new WinnerBetStateChange(playerId, action.Color);
            }
            
            return new NoActionStateChange(playerId);
        }

        public void ScoreRound()
        {
            var order = gamestate.GetLeadingOrder();
            var first = order.First();
            var second = order.Skip(1).First();

            foreach (var bet in gamestate.BettingCards.Where(x => !x.IsFree))
            {
                if (bet.CamelColor == first)
                {
                    gamestate.Money[bet.Owner] += bet.Value;
                }
                else if (bet.CamelColor == second)
                {
                    gamestate.Money[bet.Owner] += 1;
                }
                else
                {
                    if (gamestate.Money[bet.Owner] > 0)
                        gamestate.Money[bet.Owner] -= 1;
                }
            }
        }

        public void ScoreGame()
        {
            var winner = gamestate.GetLeadingOrder().First();
            var scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in gamestate.WinnerBets)
            {
                if (card.CamelColor == winner) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    gamestate.Money[card.Player] += score;
                }
                else
                {
                    if (gamestate.Money[card.Player] > 0)
                        gamestate.Money[card.Player] -= 1;
                }
            }

            var loser = gamestate.GetLeadingOrder().Last();
            scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in gamestate.LoserBets)
            {
                if (card.CamelColor == loser) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    gamestate.Money[card.Player] += score;
                }
                else
                {
                    if (gamestate.Money[card.Player] > 0)
                        gamestate.Money[card.Player] -= 1;
                }
            }

        }
        
        public List<int> GetWinners()
        {
            var max = gamestate.Money.OrderByDescending(x => x.Value).First().Value;
            return gamestate.Money.Where(x => x.Value == max).Select(x => x.Key).ToList();
        }
    }
}