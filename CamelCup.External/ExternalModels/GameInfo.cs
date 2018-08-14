using System;
using System.Collections.Generic;

namespace Delver.CamelCup.External
{
    public class GameInfo
    {
        public Guid GameId { get; set; }
        public RoundType RoundType { get; set; }
        public string[] Players { get; set; }
    }

    public enum RoundType 
    {
        Preliminary,
        QuarterFinal,
        SemiFinal,
        Final
    }
}
