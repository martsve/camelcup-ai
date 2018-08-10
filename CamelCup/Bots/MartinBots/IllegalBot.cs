using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class IllegalBot : ICamelCupPlayer
    {
        private string name;

        private bool usePlusTrap;
        private bool useMinusTrap;
        private bool betOnWinner;
        private bool betOnLoser;

        private int Me;

        public List<CamelColor> BetCardsRemaining = CamelHelper.GetAllCamelColors();

        private static Random rnd = new Random();

        public IllegalBot(int num = 1, bool usePlusTrap = true, bool useMinusTrap = true, bool betOnWinner = true, bool betOnLoser = true)
        {
            name = $"IllegalBot #{num}";

            this.usePlusTrap = usePlusTrap;
            this.useMinusTrap = useMinusTrap;
            this.betOnWinner = betOnWinner;
            this.betOnLoser = betOnLoser;
        }

        public string GetPlayerName()
        {
            return name;
        }

        public void StartNewGame(int playerId, GameInfo info, GameState gameState)
        {
            Me = playerId;
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

            switch (GetRandomAction())
            {
                case CamelAction.PickCard:

                    var bestBets = gameState.BettingCards.OrderByDescending(x => rnd.Next());
                    return new PlayerAction() { CamelAction = CamelAction.PickCard, Color = bestBets.First().CamelColor };

                case CamelAction.SecretBetOnLoser:
                    if (BetCardsRemaining.Any())
                    {
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).Last();
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, Color = bet };
                    }
                    break;

                case CamelAction.SecretBetOnWinner:
                    if (BetCardsRemaining.Any())
                    {    
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).First();
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, Color = bet };
                    }
                    break;

                case CamelAction.PlaceMinusTrap:
                    return new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = rnd.Next(0, gameState.BoardSize) };

                case CamelAction.PlacePlussTrap:
                    var plussLoc = GetRandomTrapPlace(gameState, true);
                    if (plussLoc > -1) {
                        return new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = rnd.Next(0, gameState.BoardSize) };
                    }
                    break;

                case CamelAction.ThrowDice:
                default:
                    break;
            }

            return new PlayerAction() { CamelAction = CamelAction.NoAction };
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
                { CamelAction.PlaceMinusTrap, useMinusTrap ? 1 : 0 },
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