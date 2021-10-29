using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Touch
{
    public abstract class TouchZone
    {
        public abstract float Measure(TouchProbe probe);
    }

    public class AggregateTouchZone : TouchZone
    {
        private readonly TouchZone[] _sensors;

        public AggregateTouchZone(IEnumerable<TouchZone> sensors) => _sensors = sensors.ToArray();

        public override float Measure(TouchProbe probe) => _sensors.Max(x => x.Measure(probe));
    }
}
