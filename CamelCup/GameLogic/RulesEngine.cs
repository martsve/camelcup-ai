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
        public bool Validate(GameState gameState, int playerId, PlayerAction action) 
        {
            if (action.CamelAction == CamelAction.NoAction)
                return true;

            else if (action.CamelAction == CamelAction.ThrowDice)
                return true;

            else if (action.CamelAction == CamelAction.PickCard)
            { 
                var bet = gameState.BettingCards.Where(x => x.IsFree && x.CamelColor == action.Color).FirstOrDefault();

                if (bet != null) {
                    return true;
                }
                
                return false;
            }

            else if (action.CamelAction == CamelAction.PlaceMinusTrap)
            { 
                if (action.Value < 1 || action.Value >= gameState.BoardSize) 
                    return false;

                if (gameState.Camels.Any(x => x.Location == action.Value))
                    return false;

                if (gameState.Traps.Any(x => x.Key == playerId && x.Value.Location == action.Value && x.Value.Move == 1))
                    return true;

                if (gameState.Traps.Any(x => x.Value.Location == action.Value || x.Value.Location - 1 == action.Value || x.Value.Location + 1 == action.Value))
                    return false;

                return true;
            }

            else if (action.CamelAction == CamelAction.PlacePlussTrap)
            { 
                if (action.Value < 1 || action.Value >= gameState.BoardSize) 
                    return false;
                    
                if (gameState.Camels.Any(x => x.Location == action.Value))
                    return false;
                
                if (gameState.Traps.Any(x => x.Key == playerId && x.Value.Location == action.Value && x.Value.Move == -1))
                    return true;

                if (gameState.Traps.Any(x => x.Value.Location == action.Value || x.Value.Location - 1 == action.Value || x.Value.Location + 1 == action.Value))
                    return false;
                
                return true;
            }

            else if (action.CamelAction == CamelAction.SecretBetOnLoser || action.CamelAction == CamelAction.SecretBetOnWinner)
            { 
                if (gameState.WinnerBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return false;
                if (gameState.LoserBets.Any(x => x.Player == playerId && x.CamelColor == action.Color))
                    return false;

                return true;
            }

            return false;
        }

        public void Iterate(GameState gameState, int playerId, PlayerAction action) 
        {
            if (action.CamelAction == CamelAction.NoAction)
                return;

            if (action.CamelAction == CamelAction.ThrowDice)
            {
                gameState.Money[playerId] += 1;
                ThrowDice(gameState, action);
            }

            else if (action.CamelAction == CamelAction.PickCard)
            { 
                var card = gameState.BettingCards.Where(x => x.IsFree && x.CamelColor == action.Color).OrderByDescending(x => x.Value).First();
                card.Owner = playerId;
                action.Value = card.Value;
            }

            else if (action.CamelAction == CamelAction.PlaceMinusTrap)
            { 
                gameState.Traps[playerId].Location = action.Value;
                gameState.Traps[playerId].Move = -1;
            }

            else if (action.CamelAction == CamelAction.PlacePlussTrap)
            { 
                gameState.Traps[playerId].Location = action.Value;
                gameState.Traps[playerId].Move = 1; 
            }

            else if (action.CamelAction == CamelAction.SecretBetOnLoser)
            {
                gameState.LoserBets.Add(new GameEndBet() { Player = playerId, CamelColor = action.Color });
            }
            
            else if (action.CamelAction == CamelAction.SecretBetOnWinner)
            { 
                gameState.WinnerBets.Add(new GameEndBet() { Player = playerId, CamelColor = action.Color });
            }
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

        public void MoveCamel(GameState gameState, CamelColor color, int value) 
        {
            gameState.RemainingDice.Remove(color);

            var mainCamel = gameState.Camels.First(x => x.CamelColor == color);
            var camelStack = gameState.Camels.Where(x => x.Location == mainCamel.Location && x.Height >= mainCamel.Height).ToList();
            camelStack.ForEach(x => x.Height += 500);

            var oldLocation = mainCamel.Location;
            var newLocation = mainCamel.Location + value;

            var trap = gameState.Traps.FirstOrDefault(x => x.Value.Location == newLocation);
            if (trap.Value != null) 
            {
                gameState.Money[trap.Key] += 1;
                newLocation += trap.Value.Move;

                if (trap.Value.Move == -1) 
                {
                    camelStack.ForEach(x => x.Height -= 1000);
                }
            }

            camelStack.ForEach(x => x.Location = newLocation);

            ResetStackHeigh(gameState.Camels);
        }

        private Random rnd = new Random();
        private void ThrowDice(GameState gameState, PlayerAction action) 
        {
            var index = rnd.Next(0, gameState.RemainingDice.Count);
            var color = gameState.RemainingDice[index];
            var value = rnd.Next(1, 4);

            action.Color = color;
            action.Value = value;

            MoveCamel(gameState, color, value);
        }

        private void ResetStackHeigh(List<Camel> camels)
        {
            foreach (var group in camels.GroupBy(x => x.Location))
            {
                var height = 0;
                foreach (var camel in group.OrderBy(x => x.Height).ToList())
                {
                    camel.Height = height;
                    height++;
                }
            }
        }
    }
}