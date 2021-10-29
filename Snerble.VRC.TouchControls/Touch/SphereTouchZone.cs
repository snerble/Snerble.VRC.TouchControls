using UnityEngine;

namespace Snerble.VRC.TouchControls.Touch
{
    public class SphereTouchZone : TouchZone
    {
        public SphereTouchZone()
        {
        }

        public SphereTouchZone(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public virtual Vector3 Position { get; }
        public virtual float Radius { get; }

        public override float Measure(TouchProbe probe)
        {
            return SphereUtils.GetIntersectionAmount(
                Position, Radius,
                probe.Position, probe.Radius);
        }
    }
}
