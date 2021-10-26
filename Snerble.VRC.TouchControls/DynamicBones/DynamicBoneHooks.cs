using System;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.DynamicBones
{
    public static class DynamicBoneHooks
    {
        //[Hook]
        public static void OnConfigureHarmony(HarmonyLib.Harmony harmony)
        {
            Log.Msg("[{0}] Applying dynamic bone patches...", nameof(DynamicBoneHooks));

            harmony.Patch(
                DynamicBoneConstants.OnEnableMethod,
                postfix: ((Action<DynamicBone>)OnEnablePostfix).ToHarmony());

            harmony.Patch(
                DynamicBoneConstants.OnDisableMethod,
                postfix: ((Action<DynamicBone>)OnDisablePostfix).ToHarmony());
        }

        private static void OnEnablePostfix(DynamicBone __instance)
        {
            Log.Msg("Enabled dynamic bones at: {0}", __instance.GetPath());
        }

        private static void OnDisablePostfix(DynamicBone __instance)
        {
            Log.Msg("Disabled dynamic bones at: {0}", __instance.GetPath());
        }
    }
}
