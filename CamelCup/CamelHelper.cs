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

        public static void Echo(CamelCupGame game)
        {
            if (StepTrough)
                Console.WriteLine(game.ToString());
        }

        public static void Pause()
        {
            if (StepTrough)
                Console.ReadLine();
        }

        public static List<CamelColor> GetLeadingOrder(this GameState gameState) 
        {
            return gameState.Camels.OrderByDescending(x => x.Height).Select(x => x.CamelColor).ToList();
        }

        public static List<CamelColor> GetAllCamelColors()
        {
            return Enum.GetValues(typeof(CamelColor)).Cast<CamelColor>().OrderBy(x => (int)x).ToList();
        }

        public static bool IsValidTrapSpace(this GameState gameState, int playerId, int location, bool positive = true)
        {
            if (location < 1 || location >= gameState.BoardSize) 
                return false;
                
            if (gameState.Camels.Any(x => x.Location == location))
                return false;
            
            var legalOwnTrapValue = positive ? -1 : 1;

            if (gameState.Traps.Any(x => x.Key == playerId && ((x.Value.Location == location && x.Value.Move == legalOwnTrapValue) || x.Value.Location - 1 == location || x.Value.Location + 1 == location)))
                return true;

            if (gameState.Traps.Any(x => x.Value.Location == location || x.Value.Location - 1 == location || x.Value.Location + 1 == location))
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
                if (playerTrapPair.Key == player && expectedMove != playerTrapPair.Value.Move)
                {
                    continue;
                }

                freeLocations.Remove(playerTrapPair.Value.Location);

                if (playerTrapPair.Key == player)
                {
                    continue;
                }

                freeLocations.Remove(playerTrapPair.Value.Location - 1);
                freeLocations.Remove(playerTrapPair.Value.Location + 1);                
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
            var colors = gameState.RemainingDice.ToList();
            foreach (var dice in colors)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var change = new DiceThrowStateChange(0, dice, i);
                    gameState.Apply(change);

                    if (!gameState.RemainingDice.Any() || gameState.Camels.Any(x => x.Location >= gameState.BoardSize)) {
                        result.Add(gameState.Clone());
                    }
                    else if (depth > 0) {
                        if (includeAllStates)
                            result.Add(gameState.Clone());
                        result.AddRange(GetAllCamelEndStates(gameState, depth - 1, includeAllStates));
                    } 
                    else if (depth == 0)  {
                        result.Add(gameState.Clone());
                    }

                    gameState.Revert(change);
                }
            }
            
            return result;
        }
    }
}