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
        protected int Value { get; set; }

        protected StateAction Action { get; set; }

        protected CamelColor Color { get; set; }

        protected int Player { get; set; }

        public StateChange(StateAction action, int player, CamelColor color, int value)
        {
            Action = action;
            Player = player;
            Color = color;
            Value = value;
        }

        public virtual void Apply(GameState state)
        {
        }

        public virtual void Reverse(GameState state) 
        {
        }
    }

    public class StateBuilder 
    {
        public GameState GameState { get { return _state; } }

        private GameState _state { get; set; }
        
        public StateBuilder()
        {
            _state = new GameState();
        }

        public StateBuilder(GameState state)
        {
            _state = state;
        }

        public GameState Build(List<StateChange> changes)
        {
            return Modify(changes, _state.Clone());
        }

        public GameState Apply(List<StateChange> changes)
        {
            return Modify(changes, _state);
        }

        public GameState Apply(StateChange change)
        {
            change.Apply(_state);
            return _state;
        }

        private GameState Modify(List<StateChange> changes, GameState state)
        {
            foreach (var change in changes)
            {
                change.Apply(state);
            }

            return state;
        }
    }
}