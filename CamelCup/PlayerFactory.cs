

using System;
using System.Reflection;
using Delver.CamelCup.External;

namespace Delver.CamelCup
{
    public class PlayerInterfaceFactory {
        public static ICamelCupPlayer CreateByName(string name, params object[] args)
        {
            Type playerType = Type.GetType(name);
            ICamelCupPlayer instance = (ICamelCupPlayer)Activator.CreateInstance(playerType, args);
            return instance;
        }
    }
}