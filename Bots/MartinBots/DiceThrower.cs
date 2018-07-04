using System;
using Delver.CamelCup;

namespace Delver.CamelCup.MartinBots
{
    public class DiceThrower : ICamelCupPlayer
    {
        public string GetPlayerName()
        {
            return "Martin The Dice Thrower";
        }

        public void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState)
        {
        }

        public void InformAboutAction(int player, PlayerAction action, GameState gameState)
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