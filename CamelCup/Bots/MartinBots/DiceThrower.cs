using System;
using System.Collections.Generic;
using Delver.CamelCup.External;

namespace Delver.CamelCup.MartinBots
{
    public class DiceThrower : ICamelCupPlayer
    {
        public string GetPlayerName()
        {
            return "Martin The Dice Thrower";
        }

        public void StartNewGame(int playerId, GameInfo info, GameState gameState)
        {
        }

        public void InformAboutAction(int player, PlayerAction action, GameState gameState)
        {
        }

        public void Winners(List<int> winners, GameState gameState)
        {
        }

        public PlayerAction GetAction(GameState gameState)
        {
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