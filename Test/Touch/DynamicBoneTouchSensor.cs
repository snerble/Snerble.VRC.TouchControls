using Snerble.VRC.TouchControls.Touch;
using System.Numerics;

namespace Test.Touch
{
    public sealed class DynamicBoneTouchSensor : AggregateTouchSensor
    {
        public sealed class BoneSegmentSensor : TouchSensor
        {
            public readonly Transform _bone;
            internal readonly float _time;
            internal readonly DynamicBone _dynamicBone;

            public Vector3 Position => _bone.position;
            public float Radius => _dynamicBone.m_RadiusDistrib.Evaluate(_time) * _dynamicBone.m_Radius;

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
            current ??= dynamicBone.m_Root;

            yield return new Tuple<int, Transform>(depth, current);

            foreach (var child in Enumerable.Range(0, current.childCount)
                .Select(i => current.GetChild(i))
                .Where(t => !dynamicBone.m_Exclusions.Contains(t)))
            {
                foreach (var item in Walk(dynamicBone, depth + 1, child))
                    yield return item;
            }
        }

        public override float Measure(TouchProbe probe)
        {
            return Sensors
                .Cast<BoneSegmentSensor>()
                .Select(sensor => new { sensor, m = sensor.Measure(probe) })
                .GroupBy(x => x.sensor._time)
                .Select(x => x.Max(x => x.m) * x.Key)
                .Max();
            //return base.Measure(probe);
        }
    }
}
