using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Touch
{
    public sealed class DynamicBoneTouchZone : AggregateTouchZone
    {
        private sealed class ColliderTouchZone : TouchZone
        {
            private readonly Transform _bone;
            private readonly float _time;
            private readonly DynamicBone _dynamicBone;

#if DEBUG
            private readonly GameObject proxy;
#endif

            public ColliderTouchZone(Transform bone, float time, DynamicBone dynamicBone)
            {
                _bone = bone;
                _time = time;
                _dynamicBone = dynamicBone;

#if DEBUG
                proxy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                proxy.SetActive(true); 
#endif
            }

            public override float Measure(TouchProbe probe)
            {
                float radius = _dynamicBone.m_Radius;
                if (_dynamicBone.m_RadiusDistrib.GetKeys().Length > 0)
                    radius *= _dynamicBone.m_RadiusDistrib.Evaluate(_time);

                var pos = _bone.position;
                var measurement = SphereUtils.GetIntersectionAmount(
                    pos, radius,
                    probe.Position, probe.Radius);

#if DEBUG
                var color = Color.Lerp(Color.red, Color.green, measurement);
                proxy.GetComponent<Renderer>().material.color = color;
                proxy.transform.localScale = new Vector3(radius, radius, radius) * 2;
                proxy.transform.position = pos;
#endif

                return measurement;
            }
        }

        public DynamicBoneTouchZone(DynamicBone dynamicBone)
            : base(GetSensors(dynamicBone))
        {
        }

        private static IEnumerable<ColliderTouchZone> GetSensors(DynamicBone dynamicBone)
        {
            var dynamicBones = Walk(dynamicBone).ToArray();
            int maxDepth = dynamicBones.Max(x => x.Item1);

            return dynamicBones
                .Select(x => new ColliderTouchZone(
                    x.Item2,
                    x.Item1 / (float)maxDepth,
                    dynamicBone));
        }

        private static IEnumerable<Tuple<int, Transform>> Walk(DynamicBone dynamicBone, int depth = 1, Transform current = null)
        {
            current = current ?? dynamicBone.m_Root;

            yield return new Tuple<int, Transform>(depth, current);

            foreach (var child in Enumerable.Range(0, current.childCount)
                .Select(i => current.GetChild(i))
                .Where(t => !dynamicBone.m_Exclusions.Contains(t)))
            {
                foreach (var item in Walk(dynamicBone, depth + 1, child))
                    yield return item;
            }
        }
    }
}
