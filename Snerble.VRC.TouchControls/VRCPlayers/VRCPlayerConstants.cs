using System.Reflection;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public static class VRCPlayerConstants
    {
        public static readonly MethodInfo AwakeMethod =
            typeof(VRCPlayer)
            .GetMethod(nameof(VRCPlayer.Awake));
    }
}
