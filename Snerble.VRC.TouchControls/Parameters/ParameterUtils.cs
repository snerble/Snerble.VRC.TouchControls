using Snerble.VRC.TouchControls.VRCPlayers;
using System.Linq;
using VRC.Playables;

namespace Snerble.VRC.TouchControls.Parameters
{
    public static class ParameterUtils
    {
        public static AvatarParameter GetByName(string name)
        {
            var avatarParams = VRCPlayerUtils.CurrentPlayer
                ?.field_Private_VRC_AnimationController_0
                ?.field_Private_IkController_0
                ?.field_Private_AvatarAnimParamController_0
                ?.field_Private_AvatarPlayableController_0
                ?.field_Private_Dictionary_2_Int32_AvatarParameter_0;

            if (avatarParams == null)
                return null;

            return avatarParams
                .entries
                .Select(x => x.value)
                .Where(x => x != null)
                .SingleOrDefault(x => x.prop_String_0.Equals(name));
        }
    }
}
