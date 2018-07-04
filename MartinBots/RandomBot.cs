using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup;

namespace Delver.CamelCup.MartinBots
{
    public class RandomBot : ICamelCupPlayer
    {
        private string name;

        private bool usePlusTrap;
        private bool useMinusTrap;
        private bool betOnWinner;
        private bool betOnLoser;

        private int Me;

        public List<CamelColor> BetCardsRemaining = CamelHelper.GetAllCamelColors();

        private static Random rnd = new Random();

        public RandomBot(int num = 1, bool usePlusTrap = true, bool useMinusTrap = true, bool betOnWinner = true, bool betOnLoser = true)
        {
            name = $"RandomBot #{num}";

            this.usePlusTrap = usePlusTrap;
            this.useMinusTrap = useMinusTrap;
            this.betOnWinner = betOnWinner;
            this.betOnLoser = betOnLoser;
        }

        public string GetPlayerName()
        {
            return name;
        }

        public void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState)
        {
            Me = playerId;
        }

        public void InformAboutAction(int player, PlayerAction action, GameState gameState)
        {
        }

        public PlayerAction GetAction(GameState gameState)
        {
            var leaders = gameState.GetLeadingOrder();

            switch (GetRandomAction())
            {
                case CamelAction.PickCard:

                    var bestBets = gameState.BettingCards.Free().OrderByDescending(x => x.Value).ThenBy(x => leaders.IndexOf(x.CamelColor));
                    var bestBet = bestBets.FirstOrDefault();

                    if (bestBet != null) 
                    {
                        return new PlayerAction() { CamelAction = CamelAction.PickCard, CamelColor = bestBet.CamelColor };
                    }
                    break;

                case CamelAction.SecretBetOnLoser:
                    if (BetCardsRemaining.Any())
                    {
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).Last();
                        BetCardsRemaining.Remove(bet);
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, CamelColor = bet };
                    }
                    break;

                case CamelAction.SecretBetOnWinner:
                    if (BetCardsRemaining.Any())
                    {    
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).First();
                        BetCardsRemaining.Remove(bet);
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, CamelColor = bet };
                    }
                    break;

                case CamelAction.PlaceNegativeTrap:
                    var minusLoc = GetRandomTrapPlace(gameState, false);
                    if (minusLoc > -1) {
                        return new PlayerAction() { CamelAction = CamelAction.PlaceNegativeTrap, TrapLocation = minusLoc };
                    }
                    break;

                case CamelAction.PlacePlussTrap:
                    var plussLoc = GetRandomTrapPlace(gameState, true);
                    if (plussLoc > -1) {
                        return new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, TrapLocation = plussLoc };
                    }
                    break;

                case CamelAction.ThrowDice:
                default:
                    break;
            }

            return new PlayerAction() { CamelAction = CamelAction.ThrowDice };
        }

        private int GetRandomTrapPlace(GameState gameState, bool positive) 
        {
            var freeLocations = gameState.GetFreeTrapSpaces(Me, positive);
            if (!freeLocations.Any()) {
                return -1;
            }
            var loc = rnd.Next(0, freeLocations.Count);
            return freeLocations[loc];
        }

        private CamelAction GetRandomAction()
        {    
            Dictionary<CamelAction, double> ActionChance = new Dictionary<CamelAction, double>()
            {
                { CamelAction.ThrowDice, 3 },
                { CamelAction.PickCard, 5 },
                { CamelAction.PlaceNegativeTrap, useMinusTrap ? 1 : 0 },
                { CamelAction.PlacePlussTrap, usePlusTrap ? 1 : 0 },
                { CamelAction.SecretBetOnLoser, betOnLoser ? 1 : 0 },
                { CamelAction.SecretBetOnWinner, betOnWinner ? 1 : 0 },
            };
            
            var totalSum = ActionChance.Select(x => x.Value).Sum();
            var rnd = new Random().NextDouble() * totalSum;
            var current = 0.0;
            foreach (var pair in ActionChance)
            {
                current += pair.Value;

                if (rnd > current)
                {
                    continue;
                }

                return pair.Key;
            }

            return CamelAction.ThrowDice;
        }

        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}