using UnityEngine;

namespace Snerble.VRC.TouchControls
{
    internal static class CurveConstants
    {
        public static readonly AnimationCurve LinearCurve = new AnimationCurve(new[]
        {
            new Keyframe(0f, 0f, 1f, 1f),
            new Keyframe(1f, 1f, 1f, 1f)
        });

        public static readonly AnimationCurve BooleanCurve = new AnimationCurve(new[]
        {
            new Keyframe(0, 0f, float.PositiveInfinity, float.PositiveInfinity),
            new Keyframe(1 - 1e-30f, 1f, float.PositiveInfinity, float.PositiveInfinity)
        });
    }
}
