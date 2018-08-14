using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class RandomBot : ICamelCupPlayer
    {
        private readonly string _name;

        private readonly bool _usePlusTrap;
        private readonly bool _useMinusTrap;
        private readonly bool _betOnWinner;
        private readonly bool _betOnLoser;

        private int _me;

        public List<CamelColor> BetCardsRemaining = CamelHelper.GetAllCamelColors();

        private static Random Rnd { get; set; }

        public RandomBot()
        {
            _name = $"RandomBot #{0}";

            _usePlusTrap = true;
            _useMinusTrap = true;
            _betOnWinner = true;
            _betOnLoser = true;

            Rnd = new Random();
        }

        public RandomBot(int num = 1, bool usePlusTrap = true, bool useMinusTrap = true, bool betOnWinner = true, bool betOnLoser = true, int? seed = null)
        {
            _name = $"RandomBot #{num}";

            _usePlusTrap = usePlusTrap;
            _useMinusTrap = useMinusTrap;
            _betOnWinner = betOnWinner;
            _betOnLoser = betOnLoser;

            Rnd = new Random(seed ?? Guid.NewGuid().GetHashCode());
        }

        public string GetPlayerName()
        {
            return _name;
        }

        public void StartNewGame(int playerId, GameInfo info, GameState gameState)
        {
            _me = playerId;
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

                    var bestBets = gameState.BettingCards.Where(x => x.IsFree).OrderByDescending(x => x.Value).ThenBy(x => leaders.IndexOf(x.CamelColor));
                    var bestBet = bestBets.FirstOrDefault();

                    if (bestBet != null) 
                    {
                        return new PlayerAction() { CamelAction = CamelAction.PickCard, Color = bestBet.CamelColor };
                    }
                    break;

                case CamelAction.SecretBetOnLoser:
                    if (BetCardsRemaining.Any())
                    {
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).Last();
                        BetCardsRemaining.Remove(bet);
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnLoser, Color = bet };
                    }
                    break;

                case CamelAction.SecretBetOnWinner:
                    if (BetCardsRemaining.Any())
                    {    
                        var bet = BetCardsRemaining.OrderBy(x => leaders.IndexOf(x)).First();
                        BetCardsRemaining.Remove(bet);
                        return new PlayerAction() { CamelAction = CamelAction.SecretBetOnWinner, Color = bet };
                    }
                    break;

                case CamelAction.PlaceMinusTrap:
                    var minusLoc = GetRandomTrapPlace(gameState, false);
                    if (minusLoc > -1) {
                        return new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = minusLoc };
                    }
                    break;

                case CamelAction.PlacePlussTrap:
                    var plussLoc = GetRandomTrapPlace(gameState, true);
                    if (plussLoc > -1) {
                        return new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = plussLoc };
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
            var freeLocations = gameState.GetFreeTrapSpaces(_me, positive);
            if (!freeLocations.Any()) {
                return -1;
            }
            
            var loc = Rnd.Next(0, freeLocations.Count);
            return freeLocations[loc];
        }

        private CamelAction GetRandomAction()
        {    
            var actionChance = new Dictionary<CamelAction, double>()
            {
                { CamelAction.ThrowDice, 3 },
                { CamelAction.PickCard, 5 },
                { CamelAction.PlaceMinusTrap, _useMinusTrap ? 1 : 0 },
                { CamelAction.PlacePlussTrap, _usePlusTrap ? 1 : 0 },
                { CamelAction.SecretBetOnLoser, _betOnLoser ? 1 : 0 },
                { CamelAction.SecretBetOnWinner, _betOnWinner ? 1 : 0 },
            };
            
            var totalSum = actionChance.Select(x => x.Value).Sum();
            var rnd = Rnd.NextDouble() * totalSum;
            var current = 0.0;
            foreach (var pair in actionChance)
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