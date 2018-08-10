using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class HeatmapPluss : ICamelCupPlayer
    {
        private int Me;
        private static Random rnd = new Random();

        public string GetPlayerName()
        {
            return "HeatmapPluss";
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
            var myTrap = gameState.Traps.FirstOrDefault(x => x.Key == Me && x.Value.Location > -1);

            if (myTrap.Value == null) 
            {
                var endStates = CamelHelper.GetAllCamelEndStates(gameState, 2, true);
                var heatmap = CamelHelper.GetHeatMap(endStates);

                foreach (var location in heatmap.Where(x => x.Value > 0.5).OrderByDescending(x => x.Value).Select(x => x.Key))
                {
                    if (gameState.IsValidTrapSpace(Me, location))
                    {
                        return new PlayerAction() { CamelAction = CamelAction.PlacePlussTrap, Value = location };
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