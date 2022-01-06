using Snerble.VRC.TouchControls.VRCPlayers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
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

        public static AnimatorControllerParameterProxy GetLocalByName(string name)
        {
            var parameters = VRCPlayerUtils
                .CurrentPlayer
                ?.field_Private_AnimatorControllerManager_0
                ?.field_Private_AvatarAnimParamController_0
                ?.field_Private_AvatarPlayableController_0
                ?.field_Private_ArrayOf_AvatarAnimLayer_0
                .Where(x => x != null)
                .Where(x => x.field_Private_RuntimeAnimatorController_0 != null)
                .Select(x => x.field_Private_AnimatorControllerPlayable_0)
                .SelectMany(x => Enumerable
                    .Range(0, x.GetParameterCount())
                    .Select(i => Tuple.Create(x, x.GetParameter(i))))
                .GroupBy(x => x.Item2?.name)
                .SingleOrDefault(x => x.Key == name)
                ?.ToArray();

            if (parameters == null)
                return default;

            return new AnimatorControllerParameterProxy(
                parameters.Select(x => x.Item1).ToArray(),
                parameters.First().Item2);
        }
    }

    public struct AnimatorControllerParameterProxy
    {
        public AnimatorControllerParameterProxy(
            AnimatorControllerPlayable[] controllers,
            AnimatorControllerParameter parameter)
        {
            Controllers = controllers;
            Parameter = parameter;
        }

        public AnimatorControllerPlayable[] Controllers { get; }
        public AnimatorControllerParameter Parameter { get; }

        public float Get()
        {
            switch (Parameter.type)
            {
                case AnimatorControllerParameterType.Float:
                    return Controllers.FirstOrDefault().GetFloat(Parameter.nameHash);
                case AnimatorControllerParameterType.Int:
                    return Controllers.FirstOrDefault().GetInteger(Parameter.nameHash);
                case AnimatorControllerParameterType.Bool:
                    return Convert.ToSingle(Controllers.FirstOrDefault().GetBool(Parameter.nameHash));
                default:
                    throw new NotImplementedException();
            }
        }

        public void Set(float value)
        {
            foreach (var controller in Controllers)
            {
                switch (Parameter.type)
                {
                    case AnimatorControllerParameterType.Float:
                        controller.SetFloat(Parameter.nameHash, value);
                        break;
                    case AnimatorControllerParameterType.Int:
                        controller.SetInteger(Parameter.nameHash, (int)value);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        controller.SetBool(Parameter.nameHash, value == 1f);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
