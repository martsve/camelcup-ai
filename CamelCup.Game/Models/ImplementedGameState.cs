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
                Disqualified[i] = false;
            }

            var h = 0;
            foreach (var item in startingPositions.OrderBy(x => x.Value.Location).ThenBy(x => x.Value.Height))
            {
                Camels.Add(new Camel() {
                    CamelColor = item.Key,
                    Location = item.Value.Location,
                    Height = h++,
                });
            }
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
    }
}
