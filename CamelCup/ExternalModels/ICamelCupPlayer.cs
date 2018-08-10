using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup.External
{
    public class GameInfo
    {
        public Guid GameId { get; set; }
        public string[] Players { get; set; }
    }

    public interface ICamelCupPlayer 
    {
        string GetPlayerName();

        void StartNewGame(int playerId, GameInfo info, GameState gameState);

        void InformAboutAction(int player, PlayerAction action, GameState gameState);

        void Winners(List<int> winners, GameState gameState);

        PlayerAction GetAction(GameState gameState);

        void Save();

        void Load();
    }
}
