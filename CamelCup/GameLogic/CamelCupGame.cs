using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class CamelCupGame 
    {
        public Guid GameId { get; } = Guid.NewGuid();

        public ImplementedGameState GameState { get; set; }

        public int CurrentPlayer { get; set; }

        public int StartingPlayer { get; set; }

        private List<Player> Players { get; set; }        

        private RulesEngine RulesEngine = new RulesEngine();

        public StateBuilder StateBuilder { get; set; }

        public CamelCupGame(List<Player> players, Dictionary<CamelColor, Position> startingPositions, int seed = -1) 
        {
            Players = players;
            CurrentPlayer = 0;

            GameState = new ImplementedGameState(Players.Count, startingPositions);
            StateBuilder = new StateBuilder(GameState);

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            RulesEngine = new RulesEngine(seed);
        }

        public void StartGame() 
        {
            var playerNames = Players.Select(x => x.Name).ToArray();
            var gameStateClone = GameState.Clone(true);

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
            ImplementedPlayerAction action = new ImplementedPlayerAction();

            if (!Players[CurrentPlayer].Disqualified) {
                Attempt(() => {
                    var gameStateClone = GameState.Clone(true);
                    Players[CurrentPlayer].PerformAction(x => action = new ImplementedPlayerAction(x.GetAction(gameStateClone)));
                });
            }

            MoveGame(CurrentPlayer, action);

            foreach (var player in Players) 
            {
                Attempt(() => {
                    var gameStateClone = GameState.Clone(true);
                    var actionClone = action.Clone();
                    player.PerformAction(x => x.InformAboutAction(CurrentPlayer, actionClone, gameStateClone));
                });
            }

            if (!GameState.RemainingDice.Any()) 
            {
                RulesEngine.ScoreRound(GameState);
            }

            if (IsComplete())
            {
                foreach (var player in Players) 
                {
                    RulesEngine.ScoreGame(GameState);
                    var winners = RulesEngine.GetWinners(GameState);
                    Attempt(() => {
                        var gameStateClone = GameState.Clone(true);
                        player.PerformAction(x => x.Winners(winners, gameStateClone));
                    });
                }
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
            GameState.BettingCards = ImplementedBettingCard.GetAllBettingCards();

            foreach (var playerTrapPair in GameState.Traps)
                playerTrapPair.Value.Location = -1;            
        }

        private void MoveGame(int player, PlayerAction action)
        {
            var change = RulesEngine.Getchange(GameState, player, action);

            if (change != null) 
            {
                StateBuilder.Apply(change);
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
                GameState.Money[ex.PlayerId] = 0;
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