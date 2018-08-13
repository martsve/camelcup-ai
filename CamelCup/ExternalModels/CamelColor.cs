﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Delver.CamelCup.External
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CamelColor
    {
        [EnumMember(Value = "Blue")]
        Blue = 0,
        [EnumMember(Value = "Green")]
        Green = 1,
        [EnumMember(Value = "Orange")]
        Orange = 2,
        [EnumMember(Value = "Red")]
        Red = 3,
        [EnumMember(Value = "Yellow")]
        Yellow = 4
    }
}