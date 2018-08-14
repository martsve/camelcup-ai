using System.Collections.Generic;
using System.Runtime.Serialization;
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
        NoAction = 0,
        [EnumMember(Value = "ThrowDice")]
        ThrowDice = 1,
        [EnumMember(Value = "PickCard")]
        PickCard = 2,
        [EnumMember(Value = "PlacePlussTrap")]
        PlacePlussTrap = 3,
        [EnumMember(Value = "PlaceMinusTrap")]
        PlaceMinusTrap = 4,
        [EnumMember(Value = "SecretBetOnWinner")]
        SecretBetOnWinner = 5,
        [EnumMember(Value = "SecretBetOnLoser")]
        SecretBetOnLoser = 6,

        // game changes
        [EnumMember(Value = "StartPosition")]
        StartPosition = 7,
        [EnumMember(Value = "NewRound")]
        NewRound = 8,
        [EnumMember(Value = "GetMoney")]
        GetMoney = 9,
        [EnumMember(Value = "Move")]
        Move = 10,
        [EnumMember(Value = "Disqualified")]
        Disqualified = 11
    }

    public class StateChange 
    {
        public int Value { get; set; }

        public StateAction Action { get; set; }

        public CamelColor Color { get; set; }

        public int Player { get; set; }

        [JsonIgnore]
        public List<StateChange> ChildChanges { get; set; }

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