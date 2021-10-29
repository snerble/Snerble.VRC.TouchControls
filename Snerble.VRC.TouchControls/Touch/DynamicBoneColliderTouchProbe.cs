using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Snerble.VRC.TouchControls.Touch
{
    public sealed class DynamicBoneColliderTouchProbe : TouchProbe
    {
        private readonly DynamicBoneCollider _collider;

#if DEBUG
        private readonly GameObject proxy;
#endif

        public DynamicBoneColliderTouchProbe(DynamicBoneCollider collider)
        {
            _collider = collider;
#if DEBUG
            proxy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            proxy.SetActive(true);
#endif
        }

#if DEBUG
        public override Vector3 Position
        {
            get
            {
                var pos = _collider.transform.position + _collider.m_Center;
                var radius = Radius;

                proxy.transform.localScale = new Vector3(radius, radius, radius) * 2;
                proxy.transform.position = pos;
                return pos;
            }
        }
#else
        public override Vector3 Position => _collider.transform.position + _collider.m_Center;

#endif
        public override float Radius => _collider.m_Radius * _collider.transform.lossyScale.x;

        public static IEnumerable<DynamicBoneColliderTouchProbe> FromDynamicBones(DynamicBone dynamicBone)
        {
            return dynamicBone.m_Colliders
                .ToArray()
                .Select(x => new DynamicBoneColliderTouchProbe(x));
        }
    }
}
