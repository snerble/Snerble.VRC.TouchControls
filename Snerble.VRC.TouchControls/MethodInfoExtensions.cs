using HarmonyLib;
using System;
using System.Reflection;

namespace Snerble.VRC.TouchControls
{
    public static class MethodInfoExtensions
    {
        public static HarmonyMethod ToHarmony(this MethodInfo method) => new HarmonyMethod(method);
        public static HarmonyMethod ToHarmony(this Delegate @delegate) => new HarmonyMethod(@delegate.Method);
    }
}
