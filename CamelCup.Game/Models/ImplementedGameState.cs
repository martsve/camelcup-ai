using System.Collections.Generic;
using System.Linq;
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

            for (var i = 0; i < players; i++)
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

        public GameState Clone(int player) 
        {
            var serialized = JsonConvert.SerializeObject(this);
            var clone = JsonConvert.DeserializeObject<GameState>(serialized);

            foreach (var bet in clone.WinnerBets.Where(x => x.Player != player))
                bet.CamelColor = null;

            foreach (var bet in clone.LoserBets.Where(x => x.Player != player))
                bet.CamelColor = null;

            return clone;
        }
    }
}
