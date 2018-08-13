using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Delver.CamelCup.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Delver.CamelCup
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateAction
    {
        // player actions
        [EnumMember(Value = "NoAction")]
        NoAction,
        [EnumMember(Value = "ThrowDice")]
        ThrowDice,
        [EnumMember(Value = "PickCard")]
        PickCard,
        [EnumMember(Value = "PlacePlussTrap")]
        PlacePlussTrap,
        [EnumMember(Value = "PlaceMinusTrap")]
        PlaceMinusTrap,
        [EnumMember(Value = "SecretBetOnWinner")]
        SecretBetOnWinner,
        [EnumMember(Value = "SecretBetOnLoser")]
        SecretBetOnLoser,

        // game changes
        [EnumMember(Value = "GetMoney")]
        GetMoney,
        [EnumMember(Value = "Move")]
        Move,
        [EnumMember(Value = "Disqualified")]
        Disqualified
    }

    public class StateChange 
    {
        public int Value { get; set; }

        public StateAction Action { get; set; }

        public CamelColor Color { get; set; }

        public int Player { get; set; }

        internal List<StateChange> ChildChanges { get; set; }

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

        public virtual void Revert(GameState state) 
        {
        }
    }
}