using UnityEngine;

namespace Snerble.VRC.TouchControls.Touch
{
    public class ObjectTouchProbe : TouchProbe
    {
        private readonly Transform _transform;
        private readonly float _radius;

        public ObjectTouchProbe(Transform transform, float radius)
        {
            _transform = transform;
            _radius = radius;
        }

        public override Vector3 Position => _transform.position;
        public override float Radius => _radius;
    }
}
