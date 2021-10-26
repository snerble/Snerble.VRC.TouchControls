using HarmonyLib;
using System.Reflection;

namespace Snerble.VRC.TouchControls.DynamicBones
{
    public static class DynamicBoneConstants
    {
        public static readonly MethodInfo OnEnableMethod = typeof(DynamicBone)
            .GetMethod(nameof(DynamicBone.OnEnable));

        public static readonly MethodInfo OnDisableMethod = typeof(DynamicBone)
            .GetMethod(nameof(DynamicBone.OnDisable));
    }
}
