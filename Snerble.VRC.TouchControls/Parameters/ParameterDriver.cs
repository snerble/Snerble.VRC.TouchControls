using System;
using VRC.Playables;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.Parameters
{
    public abstract class ParameterDriver
    {
        public abstract void Update();

        public static ParameterDriver GetFromDynamicBone(DynamicBone d)
        {
            var settings = ParameterDriverSettings.FromName(d.gameObject.name);
            if (settings == null)
                return null;

            var avatarParam = ParameterUtils.GetByName(settings.ParamName);
            if (avatarParam == null)
            {
                Log.Warning("Failed to bind '{0}': No such parameter '{1}'",
                    d.gameObject.name,
                    settings.ParamName);
                return null;
            }

#if DEBUG
            Log.Msg("{0}: {1}", nameof(settings.ParamName), settings.ParamName);
            Log.Msg("{0}: {1}", nameof(settings.IsToggle), settings.IsToggle);
            Log.Msg("{0}: {1}", nameof(settings.TargetValue), settings.TargetValue);
            Log.Msg("{0}: {1}", nameof(settings.Threshold), settings.Threshold); 
#endif

            switch (avatarParam.field_Private_EnumNPublicSealedvaUnBoInFl5vUnique_0)
            {
                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool:
                    return new BoolParameterDriver(settings, avatarParam, d);
                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Int:
                    return new IntParameterDriver(settings, avatarParam, d);
                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Float:
                    return new FloatParameterDriver(settings, avatarParam, d);
                default:
                    throw new Exception(
                        $"Unknown parameter type '{avatarParam.field_Private_EnumNPublicSealedvaUnBoInFl5vUnique_0}'");
            }
        }
    }
}
