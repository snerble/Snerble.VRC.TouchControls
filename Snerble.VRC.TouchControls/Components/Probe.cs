using System;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Components
{
    public sealed class Probe : ComponentBase
    {
        public float Radius;

#if DEBUG
        public Probe(GameObject gameObject, float radius) : base(gameObject)
        {
            Radius = radius;
        }
#endif
        public Probe(GameObject gameObject) : base(gameObject) { }

        public DynamicBoneCollider Collider => GameObject.GetComponent<DynamicBoneCollider>();

        public override void Awake()
        {
            if (Collider == null)
                throw new Exception($"Missing {typeof(DynamicBoneCollider)} component");
            Radius = Collider.m_Radius;
        }

        public float Measure(Sphere sphere) => ToSphere().Intersect(sphere);

        public Sphere ToSphere() => new Sphere(Transform.position, Radius * Transform.lossyScale.x);
    }
}
