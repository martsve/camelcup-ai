using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class SmartPluss : ICamelCupPlayer
    {
        private int Me;
        private static Random rnd = new Random();

        public SmartPluss(int seed = -1)
        {

        }

        public string GetPlayerName()
        {
            return "SmartPluss";
        }

        public void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState)
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
            var bestAction = new PlayerAction() { CamelAction = CamelAction.ThrowDice };
            var myTrap = gameState.Traps.FirstOrDefault(x => x.Key == Me && x.Value.Location > -1);
            if (myTrap.Value == null) 
            {
                var freeLocations = gameState.GetFreeTrapSpaces(Me, true);
                var engine = new RulesEngine();
                var bestSum = 0;
                foreach (var location in freeLocations) {
                    var newState = gameState.Apply(new PlussTrapStateChange(Me, location));
                    var endStates = CamelHelper.GetAllCamelEndStates(newState, 2);
                    var sum = endStates.Sum(x => x.Money.First(y => y.Key == Me).Value);
                    if (sum > bestSum) {
                        bestSum = sum;
                        bestAction = new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = location };
                    }
                }
            }

            return bestAction;
        }

        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}