using System;
using System.Collections.Generic;
using System.Linq;

using Delver.CamelCup;

namespace Delver.CamelCup
{
    public static class CamelHelper
    {        
        public static void Echo(string str)
        {
            if (true) {
                Console.WriteLine(str);
            }
        }

        public static List<CamelColor> GetLeadingOrder(this GameState gameState) 
        {
            return gameState.Camels.OrderByDescending(x => x.Location).ThenByDescending(x => x.Height).Select(x => x.CamelColor).ToList();
        }    

        public static List<BettingCard> Free(this List<BettingCard> bettingCards, Func<BettingCard, bool> predicate = null) 
        {
            return bettingCards.Where(x => x.IsFree && (predicate == null || predicate(x))).ToList();
        }  

        public static List<CamelColor> GetAllCamelColors()
        {
            return Enum.GetValues(typeof(CamelColor)).Cast<CamelColor>().ToList();
        }

        public static List<int> GetFreeTrapSpaces(this GameState gameState, int player, bool positive)
        {
            var min = gameState.Camels.Min(x => x.Location) + 1;
            var max = gameState.Camels.Max(x => x.Location) + 3;

            if (min < 1) 
            {
                min = 1;
            }

            if (max >= gameState.BoardSize) 
            {
                max = gameState.BoardSize - 1;
            }

            var freeLocations = Enumerable.Range(min, max - min + 1).ToList();

            foreach (var camel in gameState.Camels)
            {
                freeLocations.Remove(camel.Location);
            }

            int expectedMove = positive ? 1 : -1;
            foreach (var playerTrapPair in gameState.Traps.Where(x => x.Value.Location > -1))
            {
                freeLocations.Remove(playerTrapPair.Value.Location - 1);
                freeLocations.Remove(playerTrapPair.Value.Location + 1);

                if (playerTrapPair.Key == player && expectedMove != playerTrapPair.Value.Move)
                {
                    continue;
                }
                
                freeLocations.Remove(playerTrapPair.Value.Location);
            }

            return freeLocations;
        }
    }
}