using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;
using System.Diagnostics;

namespace Delver.CamelCup
{
    class CamelCupGame 
    {
        public Guid GameId { get; } = Guid.NewGuid();

        public GameState GameState { get; set; }

        public int CurrentPlayer { get; set; }

        public int StartingPlayer { get; set; }

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

                Attempt(() => {
                    player.PerformAction(x => player.Name = x.GetPlayerName());
                    player.PerformAction(x => x.StartNewGame(i, GameId, playerNames, gameStateClone));
                });
            }
        }

        public bool IsComplete() 
        {
            // one or less players remaining
            if (Players.Count(x => x.Disqualified) >= Players.Count - 1)
                return true;

            // somebody passed start
            if (GameState.Camels.Any(x => x.Location >= GameState.BoardSize))
                return true;

            return false;
        }

        public void MoveNextPlayer()
        {
            PlayerAction action = new PlayerAction();

            if (!Players[CurrentPlayer].Disqualified) {
                Attempt(() => {
                    var clone = GameState.Clone();
                    Players[CurrentPlayer].PerformAction(x => action = x.GetAction(clone));                    
                });
            }

            MoveGame(CurrentPlayer, action);

            foreach (var player in Players) 
            {
                Attempt(() => {
                    var clone = GameState.Clone();
                    var actionClone = action.Clone();
                    player.PerformAction(x => x.InformAboutAction(CurrentPlayer, actionClone, clone));
                });
            }

            if (!GameState.RemainingDice.Any()) 
            {
                RulesEngine.ScoreRound(GameState);
            }

            if (IsComplete())
            {
                RulesEngine.ScoreGame(GameState);
            }

            if (GameState.RemainingDice.Any()) 
            {
                CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
            }
            else
            {
                ResetRound();
            }
        }

        private void ResetRound()
        {
            StartingPlayer = (StartingPlayer + 1) % Players.Count;
            CurrentPlayer = StartingPlayer;
            GameState.RemainingDice = CamelHelper.GetAllCamelColors();
            GameState.BettingCards = BettingCard.GetAllBettingCards();

            foreach (var playerTrapPair in GameState.Traps)
                playerTrapPair.Value.Location = -1;            
        }

        private void MoveGame(int player, PlayerAction action)
        {
            var valid = RulesEngine.Validate(GameState, player, action);

            if (valid) 
            {
                RulesEngine.Iterate(GameState, player, action);
                CamelHelper.Echo($"\n > {player} performs {action.CamelAction} ({action.Color}/{action.Value})\n  ");
            }
            else
            {
                Players[player].Disqualified = true;
                Trace.WriteLine($"Game {GameId}: Player {player} loses the game to illegal action: {action.CamelAction} ({action.Color}/{action.Value})");
            }
        }
        
        private void Attempt(Action action) 
        {
            try {
                action.Invoke();
            }
            catch (PlayerLoseesException ex) {
                Trace.WriteLine($"Game {GameId}: Player {ex.PlayerId} loses the game to Exception/Timeout");
                Players[ex.PlayerId].Disqualified = true;
            }
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