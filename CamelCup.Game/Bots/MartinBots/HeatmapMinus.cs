using System;
using System.Collections.Generic;
using System.Linq;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class HeatmapMinus : ICamelCupPlayer
    {
        private int _me;
        
        public string GetPlayerName()
        {
            return "HeatmapMinus";
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
            if (gameState.Traps[_me].Location == -1) 
            {
                var heatmap = CamelHelper.GetLocationHeatmap(gameState, 4);

                foreach (var location in heatmap.Where(x => x.Value > 1).OrderByDescending(x => x.Value).Select(x => x.Key))
                {
                    if (gameState.IsValidTrapSpace(_me, location))
                    {
                        return new PlayerAction() { CamelAction = CamelAction.PlaceMinusTrap, Value = location };
                    }
                }
            }

            return new PlayerAction() { CamelAction = CamelAction.ThrowDice };
        }

        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}