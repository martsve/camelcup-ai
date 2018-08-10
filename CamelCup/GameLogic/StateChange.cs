using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public enum StateAction
    {
        NoAction,
        ThrowDice,
        PickCard,
        PlacePlussTrap,
        PlaceMinusTrap,
        SecretBetOnWinner,
        SecretBetOnLoser
    }

    public class StateChange 
    {
        public int Value { get; set; }

        public StateAction Action { get; set; }

        public CamelColor Color { get; set; }

        public int Player { get; set; }

        protected StateChange(StateAction action, int player, CamelColor color, int value)
        {
            Action = action;
            Player = player;
            Color = color;
            Value = value;
        }

        public virtual void Apply(GameState state)
        {
        }

        public virtual void Revert(GameState state) 
        {
        }
    }
}