using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class CamelCupGame 
    {
        public Guid GameId { get; }

        public ImplementedGameState GameState { get; set; }

        public int CurrentPlayer { get; set; }

        public int StartingPlayer { get; set; }

        private List<Player> Players { get; set; }        

        private RulesEngine RulesEngine { get; set; }

        public List<StateChange> History { get; set; }

        public CamelCupGame(List<Player> players, Dictionary<CamelColor, Position> startingPositions, int seed = -1) 
        {
            Players = players;
            CurrentPlayer = 0;

            GameState = new ImplementedGameState(Players.Count, startingPositions);

            if (seed == -1) 
            {
                seed = unchecked((int)DateTime.Now.Ticks);
            }

            GameId = GenerateSeededGuid(seed, players, GameState.Camels);

            RulesEngine = new RulesEngine(GameState, seed);
            History = new List<StateChange>();
        }

        private Guid GenerateSeededGuid(int gameSeed, List<Player> players, List<Camel> camels)
        {
            var playerString = string.Join(";", players.Select(x => x.Name));
            var startingPositions = camels.OrderBy(x => x.CamelColor).Select(x => $"{x.Location},{x.Height}").ToList();
            var startPosString = string.Join(";", startingPositions);

            var seedString = $"{gameSeed};{playerString};{startPosString}";

            var md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(seedString));
            var seed = BitConverter.ToInt32(hashed, 0);

            var r = new Random(seed);

            var guid = new byte[16];
            r.NextBytes(guid);
            return new Guid(guid);
        }

        public void StartGame() 
        {            
            foreach (var camel in GameState.Camels)
                History.Add(new StartPositionStateChange(camel));

            var gameStateClone = GameState.Clone(true);
            var playerNames = Players.Select(x => x.Name).ToArray();

            var gameInfo = new GameInfo() { 
                GameId = GameId,
                Players = playerNames
            };
            
            for (var i = 0; i < Players.Count; i++) 
            {
                var player = Players[i];
                player.Reset(i);
                History.Add(new StateChange(StateAction.GetMoney, player.PlayerId, CamelColor.Blue, 3));

                Attempt(() => {
                    player.PerformAction(x => x.StartNewGame(i, gameInfo, gameStateClone));
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
            var action = new ImplementedPlayerAction();

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
                History.AddRange(RulesEngine.ScoreRound());
            }

            if (IsComplete())
            {
                History.AddRange(RulesEngine.ScoreGame());
                var winners = RulesEngine.GetWinners();
                foreach (var player in Players) 
                {
                    Attempt(() => {
                        var gameStateClone = GameState.Clone(true);
                        player.PerformAction(x => x.Winners(winners.ToList(), gameStateClone));
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

        public List<Player> Winners()
        {
            return RulesEngine.GetWinners().Select(x => Players[x]).ToList();
        }

        private void ResetRound()
        {
            StartingPlayer = (StartingPlayer + 1) % Players.Count;
            CurrentPlayer = StartingPlayer;
            GameState.RemainingDice = CamelHelper.GetAllCamelColors();
            GameState.BettingCards = ImplementedBettingCard.GetAllBettingCards();
            GameState.Round++;

            History.Add(new StateChange(StateAction.NewRound, -1, CamelColor.Blue, GameState.Round));

            foreach (var playerTrapPair in GameState.Traps)
                playerTrapPair.Value.Location = -1;            
        }

        private void MoveGame(int player, PlayerAction action)
        {
            var change = RulesEngine.Getchange(player, action);

            if (change != null) 
            {
                GameState.Apply(change);
                History.Add(change);

                if (change.ChildChanges != null) 
                {
                    History.AddRange(change.ChildChanges);   
                }
            }
            else
            {
                Disqualify(player, action);
            }
        }

        private void Attempt(Action action) 
        {
            try {
                action.Invoke();
            }
            catch (PlayerLoseesException ex) {
                Disqualify(ex.PlayerId, new PlayerAction());
            }
        }
        
        private void Disqualify(int player, PlayerAction action) 
        {
            Players[player].Disqualified = true;
            GameState.Money[player] = 0;
            GameState.Disqualified[player] = true;
            History.Add(new DisqualifiedStateChange(player, action));
        }
    }
}