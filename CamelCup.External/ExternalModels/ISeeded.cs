using System;
using System.Collections.Generic;

namespace Delver.CamelCup.External
{
    public interface ISeeded 
    {
        void SetRandomSeed(int seed);
    }
}
