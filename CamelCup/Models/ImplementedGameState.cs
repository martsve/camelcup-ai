using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class ImplementedGameState : GameState
    {
        public ImplementedGameState() : base()
        {

        }

        public ImplementedGameState(int players, Dictionary<CamelColor, Position> startingPositions, int boardSize = 16, int startingMoney = 3) : this()
        {
            RemainingDice = CamelHelper.GetAllCamelColors();
            BettingCards = ImplementedBettingCard.GetAllBettingCards();

            BoardSize = boardSize;
            Round = 0;

            for (int i = 0; i < players; i++)
            {
                Money[i] = startingMoney;
                Traps[i] = new Trap();
            }

            var h = 0;
            foreach (var item in startingPositions.OrderBy(x => x.Value.Height))
            {
                Camels.Add(new Camel() {
                    CamelColor = item.Key,
                    Location = item.Value.Location,
                    Height = h++,
                });
            }
        }

        private List<Camel> GetCamelsInLocation(int location) 
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
}
