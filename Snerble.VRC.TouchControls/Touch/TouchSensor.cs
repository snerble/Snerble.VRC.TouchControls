using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Touch
{
    public class TouchSensor
    {
        public TouchSensor(DynamicBone dynamicBone) : this(
            new DynamicBoneTouchZone(dynamicBone),
            DynamicBoneColliderTouchProbe.FromDynamicBones(dynamicBone))
        {
        }

        public TouchSensor(
            TouchZone zone,
            IEnumerable<TouchProbe> probes)
        {
            Zone = zone;
            Probes = probes.ToArray();
        }

        protected TouchZone Zone { get; }
        protected TouchProbe[] Probes { get; }

        public virtual float Measure() => Probes.Max(x => Zone.Measure(x));
    }
}
