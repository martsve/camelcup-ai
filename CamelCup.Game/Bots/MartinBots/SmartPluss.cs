using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class SmartPluss : ICamelCupPlayer
    {
        private int _me;

        public SmartPluss()
        {

        }

        public string GetPlayerName()
        {
            return "SmartPluss";
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
            var bestAction = new PlayerAction() { CamelAction = CamelAction.ThrowDice };
            var myTrap = gameState.Traps.FirstOrDefault(x => x.Key == _me && x.Value.Location > -1);
            if (myTrap.Value == null) 
            {
                var freeLocations = gameState.GetFreeTrapSpaces(_me, true);
                var bestSum = 1;
                foreach (var location in freeLocations) {
                    gameState.Traps[_me].Location = location;
                    gameState.Traps[_me].Move = 1;
                    CamelHelper.GetCamelWins(gameState, out var money, depth: 2);
                    if (money[location] > bestSum) {
                        bestSum = money[location];
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