using System;

namespace Snerble.VRC.TouchControls.Parameters
{
    public sealed class LocalParameter : IParameter
    {
        private readonly AnimatorControllerParameterProxy _param;

        public LocalParameter(string name)
        {
            _param = ParameterUtils.GetLocalByName(name);
            if (_param.Parameter == null)
                throw new ArgumentException($"No such parameter '{name}'");
        }

        public string Name => _param.Parameter.name;

        public ParameterType Type
        {
            get
            {
                switch (_param.Parameter.type)
                {
                    case UnityEngine.AnimatorControllerParameterType.Float:
                        return ParameterType.Float;
                    case UnityEngine.AnimatorControllerParameterType.Int:
                        return ParameterType.Int;
                    case UnityEngine.AnimatorControllerParameterType.Bool:
                        return ParameterType.Bool;
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
                    SyncManager.Lock(_param.Parameter, this);
                else
                    SyncManager.Release(this);
            }
        }

        public void Dispose() => Locked = false;

        public float Get() => _param.Get();

        public void Set(float value) => _param.Set(value);
    }
}
