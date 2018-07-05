using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.MartinBots;

namespace Delver.CamelCup
{
    public interface ICamelCupPlayer 
    {
        string GetPlayerName();

        void StartNewGame(int playerId, Guid gameId, string[] players, GameState gameState);

        void InformAboutAction(int player, PlayerAction action, GameState gameState);

        PlayerAction GetAction(GameState gameState);

        void Save();

        void Load();
    }

    public class GameState 
    {
        public int CurrentPlayer { get; set; } = 0;

        public int BoardSize;

        public List<Camel> Camels { get; set; } = new List<Camel>();

        public Dictionary<int, Trap> Traps { get; set; } = new Dictionary<int, Trap>();

        public Dictionary<int, int> Money { get; set; } = new Dictionary<int, int>();

        public List<BettingCard> BettingCards = new List<BettingCard>();

        public List<GameEndBet> WinnerBets = new List<GameEndBet>();

        public List<GameEndBet> LoserBets = new List<GameEndBet>();

        public List<CamelColor> RemainingDice = new List<CamelColor>();

        public GameState() 
        {

        }

        public GameState(int players, Dictionary<CamelColor, int> startingPositions,  int boardSize = 16, int startingMoney = 3)
        {
            RemainingDice = CamelHelper.GetAllCamelColors();
            BettingCards = BettingCard.GetAllBettingCards();

            BoardSize = boardSize;

            for (int i = 0; i < players; i++)
            {
                Money[i] = startingMoney;
                Traps[i] = new Trap();
            }

            foreach (var item in startingPositions)
            {
                Camels.Add(new Camel() {
                    CamelColor = item.Key,
                    Location = item.Value,
                    Height = GetCamelsInLocation(item.Value).Count + 1
                });
            }
        }

        public List<Camel> GetCamelsInLocation(int location) 
        {
            return Camels.Where(x => x.Location == location).OrderBy(x => x.Height).ToList();
        }

        public GameState Clone(bool sanitizeBets = true) 
        {
            var serialized = JsonConvert.SerializeObject(this);
            var clone = JsonConvert.DeserializeObject<GameState>(serialized);

            if (sanitizeBets)
            {
                clone.WinnerBets.ForEach(x => x.CamelColor = null);
                clone.LoserBets.ForEach(x => x.CamelColor = null);
            }

            return clone;
        }

        public override string ToString()
        {
            List<string> bets = new List<string>();
            foreach (var betCards in BettingCards.Where(x => x.IsFree).GroupBy(x => x.CamelColor))
            {
                var line = $"{betCards.Key}: ";

                foreach (var bet in betCards.OrderByDescending(x => x.Value))
                {
                    line += $" {bet.Value}";
                }

                bets.Add(line);
            }
            bets.Add("");
            foreach (var betCards in BettingCards.Where(x => !x.IsFree).GroupBy(x => x.Owner))
            {
                var line = $"{betCards.Key}:";

                foreach (var bet in betCards.OrderBy(x => x.CamelColor).ThenByDescending(x => x.Value))
                {
                    line += $" {bet.CamelColor}-{bet.Value}";
                }

                bets.Add(line);
            }
            bets.Add("");
            bets.Add("");

            List<string> spaces = new List<string>();

            var maxLoc = Math.Max(BoardSize, Camels.Max(x => x.Location + 1));
            for (int i = 0; i < maxLoc; i++) {
                spaces.Add($"{i:00}:");
            }

            foreach (var playerTrapPair in Traps)
            {
                if (playerTrapPair.Value.Location > -1)
                    spaces[playerTrapPair.Value.Location] += $" P{playerTrapPair.Key} Trap {playerTrapPair.Value.Move}";
            }

            foreach (var camel in Camels.OrderBy(x => x.Location).ThenBy(x => x.Height))
            {
                spaces[camel.Location] += $" {camel.CamelColor}";
            }

            if (maxLoc > BoardSize)
                spaces.Insert(BoardSize, "~~~~~~~~~~~~~~~~");

            return string.Join("\n", bets) + string.Join("\n", spaces);
        }
    }

    public class GameEndBet 
    {
        public int Player { get; set; }

        public CamelColor? CamelColor { get; set; }
    }

    public class BettingCard 
    {
        public CamelColor CamelColor { get; set; }

        public int Value { get; set; }

        public int Owner { get; set; }

        public bool IsFree => Owner == -1;

        public static List<BettingCard> GetAllBettingCards() 
        {
            var result = new List<BettingCard>();
            foreach (var color in CamelHelper.GetAllCamelColors())
            {
                foreach (var val in new[] { 5, 3, 2 })
                    result.Add(new BettingCard() {
                        CamelColor = color,
                        Value = val,
                        Owner = -1
                    });
            }

            return result;
        }
    }

    public class Camel
    {
        public CamelColor CamelColor { get; set; }

        public int Location { get; set; }

        public int Height { get; set; }
    }

    public class Trap
    {
        public int Move { get; set; } = 0;
        public int Location { get; set; } = -1;
    }

    public class PlayerAction 
    {
        public CamelAction CamelAction { get; set; }
        public int Value { get; set; }
        public CamelColor Color { get; set; }

        public PlayerAction Clone() 
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PlayerAction>(serialized);
        }
    }

    public enum CamelAction
     {
        NoAction,
        ThrowDice,
        PickCard,
        PlacePlussTrap,
        PlaceNegativeTrap,
        SecretBetOnWinner,
        SecretBetOnLoser
    }

    public enum CamelColor
    {
        Red,
        Blue,
        Green,
        Orange,
        Yellow
    }
}
