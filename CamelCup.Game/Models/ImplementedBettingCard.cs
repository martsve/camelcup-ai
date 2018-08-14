using System.Collections.Generic;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class ImplementedBettingCard : BettingCard 
    {
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
}
