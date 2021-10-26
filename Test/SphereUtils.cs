using System.Numerics;

namespace Test;

public static class SphereUtils
{
    public static float GetIntersectionAmount(
        Vector3 p1, float r1,
        Vector3 p2, float r2)
    {
        float maxDistance = -r1 - r2;
        float distance = (p2 - p1).Length();
        return Math.Clamp((distance + maxDistance) / maxDistance, 0, 1);
    }
}
