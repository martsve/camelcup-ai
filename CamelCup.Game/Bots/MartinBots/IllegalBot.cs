using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class IllegalBot : ICamelCupPlayer, ISeeded
    {
        private readonly string _name;

        private readonly bool _usePlusTrap;
        private readonly bool _useMinusTrap;
        private readonly bool _betOnWinner;
        private readonly bool _betOnLoser;

        private int _me;

        public List<CamelColor> BetCardsRemaining = CamelHelper.GetAllCamelColors();

        private Random Rnd;

        public IllegalBot() : this(1)
        {
        }

        public IllegalBot(int num = 1, bool usePlusTrap = true, bool useMinusTrap = true, bool betOnWinner = true, bool betOnLoser = true)
        {
            _name = $"IllegalBot #{num}";

            _usePlusTrap = usePlusTrap;
            _useMinusTrap = useMinusTrap;
            _betOnWinner = betOnWinner;
            _betOnLoser = betOnLoser;
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

                    var bestBets = gameState.BettingCards.OrderByDescending(x => Rnd.Next());
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
                    return new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = Rnd.Next(0, gameState.BoardSize) };

                case CamelAction.PlacePlussTrap:
                    return new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = Rnd.Next(0, gameState.BoardSize) };

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

        public void SetRandomSeed(int seed)
        {
            Rnd = new ConsistantRandom(seed);
        }
    }
}