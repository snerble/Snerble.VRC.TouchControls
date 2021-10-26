using System.Numerics;

namespace Snerble.VRC.TouchControls.Touch
{
    public class TouchProbe
    {
        public TouchProbe()
        {
        }

        public TouchProbe(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public virtual Vector3 Position { get; }
        public virtual float Radius { get; }
    }
}
