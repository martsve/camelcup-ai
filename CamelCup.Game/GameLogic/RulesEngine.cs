using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class RulesEngine 
    {
        private Random Rnd { get; set; }

        private GameState Gamestate { get; set; }

        public RulesEngine(GameState gamestate, int seed = -1)
        {
            Gamestate = gamestate;

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            Rnd = new Random(seed);
        }

        public StateChange Getchange(int playerId, PlayerAction action) 
        {
            if (action.CamelAction == CamelAction.ThrowDice)
            {
                if (!Gamestate.RemainingDice.Any())
                {
                    return null;
                }

                var index = Rnd.Next(0, Gamestate.RemainingDice.Count);
                var color = Gamestate.RemainingDice[index];
                var value = Rnd.Next(1, 4);

                return new DiceThrowStateChange(playerId, color, value);    
            }
            else if (action.CamelAction == CamelAction.PickCard)
            { 
                if (Gamestate.BettingCards.Any(x => x.IsFree && x.CamelColor == action.Color)) {
                    var card = Gamestate.BettingCards.Where(x => x.IsFree && x.CamelColor == action.Color).OrderByDescending(x => x.Value).First();
                    return new PickCardStateChange(playerId, action.Color, card.Value);
                }
                
                return null;
            }
            else if (action.CamelAction == CamelAction.PlaceMinusTrap)
            {
                if (CamelHelper.IsValidTrapSpace(Gamestate, playerId, action.Value, positive: false))
                {
                    return new MinusTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.PlacePlussTrap)
            { 
                if (CamelHelper.IsValidTrapSpace(Gamestate, playerId, action.Value, positive: true))
                {
                    return new PlussTrapStateChange(playerId, action.Value);
                }

                return null;
            }
            else if (action.CamelAction == CamelAction.SecretBetOnLoser || action.CamelAction == CamelAction.SecretBetOnWinner)
            { 
                if (Gamestate.WinnerBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return null;

                if (Gamestate.LoserBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
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
            var order = Gamestate.GetLeadingOrder();
            var first = order.First();
            var second = order.Skip(1).First();

            foreach (var bet in Gamestate.BettingCards.Where(x => !x.IsFree && !Gamestate.Disqualified[x.Owner]))
            {
                if (bet.CamelColor == first)
                {
                    Gamestate.Money[bet.Owner] += bet.Value;
                }
                else if (bet.CamelColor == second)
                {
                    Gamestate.Money[bet.Owner] += 1;
                }
                else
                {
                    if (Gamestate.Money[bet.Owner] > 0)
                        Gamestate.Money[bet.Owner] -= 1;
                }
            }
        }

        public void ScoreGame()
        {
            var winner = Gamestate.GetLeadingOrder().First();
            var scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in Gamestate.WinnerBets.Where(x => !Gamestate.Disqualified[x.Player]))
            {
                if (card.CamelColor == winner) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    Gamestate.Money[card.Player] += score;
                }
                else
                {
                    if (Gamestate.Money[card.Player] > 0)
                        Gamestate.Money[card.Player] -= 1;
                }
            }

            var loser = Gamestate.GetLeadingOrder().Last();
            scores = new List<int>() { 8, 5, 3, 2, 1, 1, 1, 1, 1, 1, 1 };
            foreach (var card in Gamestate.LoserBets.Where(x => !Gamestate.Disqualified[x.Player]))
            {
                if (card.CamelColor == loser) 
                {
                    var score = scores.First();
                    scores.RemoveAt(0);
                    Gamestate.Money[card.Player] += score;
                }
                else
                {
                    if (Gamestate.Money[card.Player] > 0)
                        Gamestate.Money[card.Player] -= 1;
                }
            }

        }
        
        public List<int> GetWinners()
        {
            var max = Gamestate.Money.OrderByDescending(x => x.Value).First().Value;
            return Gamestate.Money.Where(x => x.Value == max).Select(x => x.Key).ToList();
        }
    }
}