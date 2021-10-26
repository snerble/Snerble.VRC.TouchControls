using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Touch
{
    public sealed class DynamicBoneTouchSensor : AggregateTouchSensor
    {
        private sealed class BoneSegmentSensor : TouchSensor
        {
            private readonly Transform _bone;
            private readonly float _time;
            private readonly DynamicBone _dynamicBone;

            public BoneSegmentSensor(Transform bone, float time, DynamicBone dynamicBone)
            {
                _bone = bone;
                _time = time;
                _dynamicBone = dynamicBone;
            }

            public override float Measure(TouchProbe probe)
            {
                float radius = _dynamicBone.m_RadiusDistrib.Evaluate(_time) * _dynamicBone.m_Radius;
                var pos = _bone.position;

                return SphereUtils.GetIntersectionAmount(
                    pos, radius,
                    probe.Position, probe.Radius);
            }
        }

        public DynamicBoneTouchSensor(DynamicBone dynamicBone)
            : base(GetSensors(dynamicBone))
        {
        }

        private static IEnumerable<BoneSegmentSensor> GetSensors(DynamicBone dynamicBone)
        {
            var dynamicBones = Walk(dynamicBone).ToArray();
            int maxDepth = dynamicBones.Max(x => x.Item1);

            return dynamicBones
                .Select(x => new BoneSegmentSensor(
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
