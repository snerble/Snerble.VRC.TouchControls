using System;

namespace Snerble.VRC.TouchControls.Parameters
{
    public interface IParameter : IDisposable
    {
        string Name { get; }
        ParameterType Type { get; }
        bool Locked { get; set; }

        float Get();
        void Set(float value);
    }

    public enum ParameterType
    {
        Float,
        Int,
        Bool
    }
}
