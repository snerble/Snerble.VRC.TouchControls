using ParamLib;
using System;
using VRC.Playables;

namespace Snerble.VRC.TouchControls.Parameters
{
    public sealed class Parameter : IParameter
    {
        private readonly AvatarParameter _avatarParam;
        private readonly BaseParam _param;

        public Parameter(string name)
        {
            _avatarParam = ParameterUtils.GetByName(name);
            if (_avatarParam == null)
                throw new ArgumentException($"No such parameter '{name}'");
            switch (Type)
            {
                default:
                case ParameterType.Float:
                    _param = new FloatBaseParam(Name);
                    break;
                case ParameterType.Int:
                    _param = new IntBaseParam(Name);
                    break;
                case ParameterType.Bool:
                    _param = new BoolBaseParam(Name);
                    break;
            }
        }

        public string Name => _avatarParam.prop_String_0;

        public ParameterType Type
        {
            get
            {
                switch (_avatarParam.field_Private_ParameterType_0)
                {
                    case AvatarParameter.ParameterType.Bool:
                        return ParameterType.Bool;
                    case AvatarParameter.ParameterType.Int:
                        return ParameterType.Int;
                    case AvatarParameter.ParameterType.Float:
                        return ParameterType.Float;
                    default:
                        return (ParameterType)(-1);
                }
            }
        }

        public bool Locked
        {
            get => SyncManager.OwnsLock(this);
            set
            {
                if (value)
                    SyncManager.Lock(_avatarParam, this);
                else
                    SyncManager.Release(this);
            }
        }

        public void Dispose() => _param.ResetParam();

        public float Get() => _avatarParam.field_Private_Single_0;

        public void Set(float value)
        {
            switch (_param)
            {
                case BoolBaseParam boolParam:
                    boolParam.ParamValue = value >= 1;
                    break;
                default:
                    _param.ParamValue = value;
                    break;
            }
        }
    }
}
