using System;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public static class VRCPlayerUtils
    {
        public static event EventHandler<VRCPlayer> CurrentPlayerReady;

        public static VRCPlayer CurrentPlayer => VRCPlayer.field_Internal_Static_VRCPlayer_0;

        [Hook]
        public static void OnApplicationStart()
        {
            VRCPlayerHooks.AvatarIsReady += VRCPlayerHooks_AvatarIsReady;
        }

        private static void VRCPlayerHooks_AvatarIsReady(object sender, VRCPlayer e)
        {
            if (e == CurrentPlayer)
                CurrentPlayerReady?.Invoke(null, e);
        }
    }
}
