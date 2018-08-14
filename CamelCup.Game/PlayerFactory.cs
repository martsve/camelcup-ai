using System;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PlayerInterfaceFactory {
        public static ICamelCupPlayer CreateByName(string name, params object[] args)
        {
            var playerType = Type.GetType(name);
            var instance = (ICamelCupPlayer)Activator.CreateInstance(playerType, args);
            return instance;
        }
    }
}