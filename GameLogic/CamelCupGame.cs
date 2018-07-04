using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;

namespace Delver.CamelCup
{
    class CamelCupGame 
    {
        public Guid GameId { get; } = Guid.NewGuid();

        public GameState GameState { get; set; }

        public int CurrentPlayer { get; set; }

        private List<Player> Players { get; set; }        

        private RulesEngine RulesEngine = new RulesEngine();

        public CamelCupGame(List<Player> players, Dictionary<CamelColor, int> startingPositions) 
        {
            Players = players;
            CurrentPlayer = 0;
            GameState = new GameState(Players.Count, startingPositions);
        }

        public void StartGame() 
        {
            var playerNames = Players.Select(x => x.Name).ToArray();
            var gameStateClone = GameState.Clone();

            for (int i = 0; i < Players.Count; i++) 
            {
                var player = Players[i];
                player.Reset(i);
                player.PerformAction(x => x.StartNewGame(i, GameId, playerNames, gameStateClone));
            }
        }

        public void MoveNextPlayer()
        {
            var gameStateClone = GameState.Clone();

            var action = Players[CurrentPlayer].PerformAction(x => x.GetAction(gameStateClone)).Clone();

            MoveGame(CurrentPlayer, action);

            gameStateClone = GameState.Clone();

            foreach (var player in Players) 
            {
                player.PerformAction(x => x.InformAboutAction(CurrentPlayer, action, gameStateClone));
            }

            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
        }

        public void MoveGame(int player, PlayerAction action)
        {
            var valid = RulesEngine.Validate(GameState, player, action);

            if (valid) 
            {
                CamelHelper.Echo($"\n > {player} performs {action.CamelAction} ({action.CamelColor}/{action.TrapLocation})\n  ");
                RulesEngine.Iterate(GameState, player, action);
            }
        }

        public bool IsComplete() 
        {
            return false;
        }

        public override string ToString()
        {
            var header = new List<string>();
            for (int i = 0; i < Players.Count; i++) 
            {
                var player = Players[i];
                var current = CurrentPlayer == i ? "Current Player" : "";
                header.Add($"{i:00} {player.Name}: Gold {GameState.Money[i]} {current}");
            }

            header.Add("");
            header.Add("");
            return string.Join("\n", header) + GameState.ToString();
        }
    }
}