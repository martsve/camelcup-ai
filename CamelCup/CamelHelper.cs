using System;
using System.Collections.Generic;
using System.Linq;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public static class CamelHelper
    {        
        public static bool StepTrough { get; set; }
        public static void Echo(string str)
        {
            if (StepTrough)
                Console.WriteLine(str);
        }

        public static void Pause()
        {
            if (StepTrough)
                Console.ReadLine();
        }

        public static List<CamelColor> GetLeadingOrder(this GameState gameState) 
        {
            return gameState.Camels.OrderByDescending(x => x.Location).ThenByDescending(x => x.Height).Select(x => x.CamelColor).ToList();
        }

        public static List<CamelColor> GetAllCamelColors()
        {
            return Enum.GetValues(typeof(CamelColor)).Cast<CamelColor>().ToList();
        }

        public static bool IsValidTrapSpace(this GameState gameState, int player, int location)
        {
            if (gameState.Camels.Any(x => x.Location == location))
                return false;

            if (gameState.Traps.Any(x => x.Value.Location == location || x.Value.Location - 1 == location || x.Value.Location + 1 == location))
                return false;

            if (location <= 0 || location >= gameState.BoardSize)
                return false;

            return true;
        }

        public static List<int> GetFreeTrapSpaces(this GameState gameState, int player, bool positive, int maxLookahead = 3)
        {
            var min = gameState.Camels.Min(x => x.Location) + 1;
            var max = gameState.Camels.Max(x => x.Location) + maxLookahead;

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

        public static List<int> GetWinners(GameState state)
        {
            var engine = new RulesEngine();
            return engine.GetWinners(state);
        }

        public static Dictionary<CamelColor, double> GetWinningProbability(List<GameState> endStates)
        {
            var result = new Dictionary<CamelColor, double>();
            var colors = GetAllCamelColors();
            foreach (var color in colors)
            {
                result[color] = 0;
            }

            double N = endStates.Count();
            foreach (var state in endStates)
            {
                var winner = state.GetLeadingOrder().First();
                result[winner] += 1 / N;
            }
            
            return result;
        }

        public static Dictionary<int, double> GetHeatMap(List<GameState> endStates)
        {
            var result = new Dictionary<int, double>();
            var boardSize = endStates.First().BoardSize;
            for (int i = 0; i < boardSize + 3; i++)
            {
                result[i] = 0;
            }

            double N = endStates.Count();
            foreach (var state in endStates)
            {
                foreach (var camel in state.Camels)
                    result[camel.Location] += 1 / N;
            }
            
            return result;
        }

        public static List<GameState> GetAllCamelEndStates(this GameState gameState, int depth = 5, bool includeAllStates = false) 
        {
            var result = new List<GameState>();
            var engine = new RulesEngine();
            foreach (var dice in gameState.RemainingDice)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var newState = gameState.Clone();
                    engine.MoveCamel(newState, dice, i);
                    if (!newState.RemainingDice.Any() || newState.Camels.Any(x => x.Location >= newState.BoardSize)) {
                        result.Add(newState);
                    }
                    else if (depth > 0) {
                        if (includeAllStates)
                            result.Add(newState);
                        result.AddRange(GetAllCamelEndStates(newState, depth - 1, includeAllStates));
                    } 
                    else if (depth == 0)  {
                        result.Add(newState);
                    }
                }
            }
            
            return result;
        }
    }
}