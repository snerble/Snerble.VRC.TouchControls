using System;
using System.Collections.Generic;
using System.Linq;
using VRC.Playables;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public static class VRCPlayerHooks
    {
        public static event EventHandler<VRCPlayer> AvatarIsReady;

        [Hook]
        public static void OnConfigureHarmony(HarmonyLib.Harmony harmony)
        {
            Log.Msg("[{0}] Applying player patches...", nameof(VRCPlayerHooks));

            harmony.Patch(
                VRCPlayerConstants.AwakeMethod,
                postfix: ((Action<VRCPlayer>)OnVRCPlayerAwake).ToHarmony());
        }

        private static void OnVRCPlayerAwake(VRCPlayer __instance)
        {
            Log.Msg("PLAYER FUCKING WOKE");

            // OnAvatarIsReady
            __instance.Method_Public_add_Void_MulticastDelegateNPublicSealedVoUnique_0(new Action(() =>
            {
                Log.Msg("AY YO {0}", __instance.prop_String_0);
                if (__instance.prop_Player_0?.prop_ApiAvatar_0 != null)
                {

                    Log.Msg("NIGGERS {0}", __instance.prop_String_0);
                    AvatarIsReady?.Invoke(null, __instance);
                }
            }));
        }

        public static void DumpExpressionParams(VRCPlayer __instance)
        {
            var avatarParams = __instance
                ?.field_Private_VRC_AnimationController_0
                ?.field_Private_IkController_0
                ?.field_Private_AvatarAnimParamController_0
                ?.field_Private_AvatarPlayableController_0
                ?.field_Private_Dictionary_2_Int32_AvatarParameter_0;

            if (avatarParams == null)
                return;

            var @params = avatarParams.entries
                .Select(x => x.value)
                .Where(x => x != null)
                .ToArray();

            Log.Msg("Params[{0}]:", @params.Length);

            var data = new List<object[]>();
            foreach (var param in @params)
            {
                object value;
                switch (param.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0)
                {
                    case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool:
                        value = param.prop_Boolean_0;
                        break;
                    case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Int:
                        value = param.prop_Int32_1;
                        break;
                    case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Float:
                        value = param.prop_Single_0;
                        break;

                    default:
                    case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Unknown:
                        throw new Exception("Unknown parameter type");
                }

                data.Add(new[]
                {
                    param.prop_String_0?.ToString(), // Name
                    param.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0.ToString(), // Param type
                    value
                });
            }

            Log.Msg("\n" + StringUtils.Table(
                new[] { "Name", "Type", "Value" },
                data.ToArray()));
        }
    }
}
