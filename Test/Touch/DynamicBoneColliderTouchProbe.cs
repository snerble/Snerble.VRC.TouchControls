using System.Numerics;
using Test;

namespace Snerble.VRC.TouchControls.Touch
{
    public sealed class DynamicBoneColliderTouchProbe : TouchProbe
    {
        public readonly DynamicBoneCollider _collider;

        public DynamicBoneColliderTouchProbe(DynamicBoneCollider collider) => _collider = collider;

        public override Vector3 Position => _collider.transform.position + _collider.m_Center;
        public override float Radius => _collider.m_Radius;

        public static IEnumerable<DynamicBoneColliderTouchProbe> FromDynamicBones(DynamicBone dynamicBone)
        {
            return dynamicBone.m_Colliders
                .ToArray()
                .Select(x => new DynamicBoneColliderTouchProbe(x));
        }
    }
}
