using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.CamelCup.External
{
    public enum CamelAction
     {
        NoAction,
        ThrowDice,
        PickCard,
        PlacePlussTrap,
        PlaceMinusTrap,
        SecretBetOnWinner,
        SecretBetOnLoser
    }
}
