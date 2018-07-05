using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup.External
{
    public interface ICamelCupPlayer 
    {
        string GetPlayerName();

        void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState);

        void InformAboutAction(int player, PlayerAction action, GameState gameState);

        void Winners(List<int> winners, GameState gameState);

        PlayerAction GetAction(GameState gameState);

        void Save();

        void Load();
    }
}
