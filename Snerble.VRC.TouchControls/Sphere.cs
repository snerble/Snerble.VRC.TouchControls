using UnityEngine;

namespace Snerble.VRC.TouchControls
{
    public struct Sphere
    {
        public Sphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public float Intersect(Sphere other)
        {
            float radiusSum = Radius + other.Radius;
            float distance = (other.Position - Position).magnitude;
            return 1 - Mathf.Clamp(distance / radiusSum, 0, 1);
        }
    }
}
