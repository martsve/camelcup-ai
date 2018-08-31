using System;
using System.Collections.Generic;
using System.Linq;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public static class CamelHelper
    {        
        public static List<CamelColor> GetLeadingOrder(this GameState gameState) 
        {
            return gameState.Camels.OrderByDescending(x => x.Height).Select(x => x.CamelColor).ToList();
        }

        public static List<CamelColor> RemainingEndGameBets(this GameState gameState, int player) 
        {
            var possible = CamelHelper.GetAllCamelColors();
            var winners = gameState.WinnerBets.Where(x => x.Player == player).Select(x => x.CamelColor).ToList();
            var losers = gameState.LoserBets.Where(x => x.Player == player).Select(x => x.CamelColor).ToList();
            return possible.Where(x => !winners.Contains(x) && !losers.Contains(x)).ToList();
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

            var expectedMove = positive ? 1 : -1;
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

        public static List<List<Camel>> GetAllGameEndStates(this GameState gameState, int depth = 5, bool includeAllStates = false) 
        {
            var positions = GetCamelEndPositions(gameState, depth, includeAll: includeAllStates);
            return positions.Select(ConvertCamelPositionToCamels).ToList();
        }
        
        public static Dictionary<CamelColor, int> GetCamelWins(this GameState gameState, out int[] money, int depth = 5) 
        {
            var positions = GetCamelEndPositions(gameState, depth, includeAll: false);

            var result = new Dictionary<CamelColor, int>();
            foreach (var camel in GetAllCamelColors())
                result.Add(camel, 0);

            money = new int[16];

            foreach (var pos in positions)
            {
                var winner = (CamelColor)Array.IndexOf(pos.Height, 4);

                for (var i = 1; i < 16; i++)
                    money[i] += pos.Money[i];

                result[winner]++;
            }

            return result;
        }

        public static Dictionary<int, double> GetLocationHeatmap(this GameState gameState, int depth = 5) 
        {
            var positions = GetCamelEndPositions(gameState, depth, includeAll: true);
            var result = new Dictionary<int, int>();
            for (var i = 0; i < gameState.BoardSize + 3; i++)
            {
                result[i] = 0;
            }

            foreach (var pos in positions)
                foreach (var loc in pos.Location)
                    result[loc]++;

            double N = result.Count();
            return result.ToDictionary(x => x.Key, x => x.Value / N);
        }

        private static List<CamelPositions> GetCamelEndPositions(GameState gameState, int depth = 5, bool includeAll = false) 
        {
            var colors = gameState.RemainingDice.ToArray();
            var initialPosition = ConvertGameStateToCamelPosition(gameState);
            var traps =  Enumerable.Range(0, 20)
                .Select(x => gameState.Traps.Any(y => y.Value.Location == x) ? gameState.Traps.First(y => y.Value.Location == x).Value.Move  : 0)
                .ToArray();
            
            var positions = GetAllPossibleCamelPositions(initialPosition, colors, traps, depth, includeAll);

            return positions;
        }

        private static CamelPositions ConvertGameStateToCamelPosition(GameState gameState)
        {
            var pos = new CamelPositions() {  };
            pos.Location = gameState.Camels.OrderBy(x => (int)x.CamelColor).Select(x => x.Location).ToArray();
            pos.Height = gameState.Camels.OrderBy(x => (int)x.CamelColor).Select(x => x.Height).ToArray();
            pos.Money = new int[16];
            return pos;
        }

        private static List<Camel> ConvertCamelPositionToCamels(CamelPositions positions)
        {
            var camels = GetAllCamelColors().Select(x => new Camel() { CamelColor = x }).ToList();

            for (var i = 0; i < positions.Location.Length; i++)
            {
                camels[i].Location = positions.Location[i];
                camels[i].Height = positions.Height[i];
            }

            return camels;
        }

        private static List<CamelPositions> GetAllPossibleCamelPositions(CamelPositions initialPosition, CamelColor[] colors, int[] traps, int depth, bool includeAll) 
        {
            var positions = new List<CamelPositions>();

            if (initialPosition.Location.Any(x => x > 15))
            {
                positions.Add(initialPosition);
                return positions;
            }

            foreach (var dice in colors)
            {
                for (var i = 1; i <= 3; i++)
                {
                    var pos = FastCamelMove(initialPosition, (int)dice, i, traps);

                    if (colors.Length == 1 || pos.Location.Any(x => x > 15))
                    {
                        positions.Add(pos);
                        continue;
                    }
                                        
                    if (includeAll || depth == 0)
                    {
                        positions.Add(pos);
                    }

                    if (depth > 0)
                    {
                        var remaining = colors.Where(x => x != dice).ToArray();
                        var other = GetAllPossibleCamelPositions(pos, remaining, traps, depth - 1, includeAll);
                        positions.AddRange(other);
                    }
                }
            }
            
            return positions;
        }

        private static CamelPositions FastCamelMove(CamelPositions initialPosition, int dice, int value, int[] traps) 
        {
            var pos = new CamelPositions() 
            {
                Location = initialPosition.Location.ToArray(),
                Height = initialPosition.Height.ToArray(),
                Money = initialPosition.Money.ToArray()
            };

            var from = pos.Location[dice];
            var fromHeight = pos.Height[dice];
            var toSpace = from + value;
            var move = 1;
            if (traps[toSpace] != 0)
            {
                pos.Money[toSpace]++;
                if (traps[toSpace] < 1)
                    move = -1;
                toSpace += move;
            }

            var n = 0;
            var m = 0;
            for (var i = 0; i < 5; i++) 
                if (pos.Location[i] == from && pos.Height[i] >= fromHeight) 
                    n++;

            for (var x = from + 1; x < toSpace; x++)
            {
                for (var i = 0; i < 5; i++)
                    if (pos.Location[i] == x) 
                    {
                        pos.Height[i] -= n;
                        m++;
                    }
            }

            if (move == 1)
            {
                for (var i = 0; i < 5; i++)
                    if (pos.Location[i] == toSpace) 
                    {
                        pos.Height[i] -= n;
                        m++;
                    }
            }

            for (var i = 0; i < 5; i++) {
                if (pos.Location[i] == from  && pos.Height[i] >= fromHeight) {
                    pos.Location[i] = toSpace;
                    pos.Height[i] += m;
                }
            }

            return pos;
        }

        struct CamelPositions {
            public int[] Location;
            public int[] Height;
            public int[] Money;
        }
    }
}