using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Touch
{
    public class TouchUnit
    {
        public TouchUnit(
            TouchSensor sensor,
            IEnumerable<TouchProbe> probes)
        {
            Sensor = sensor;
            Probes = probes.ToArray();
        }

        protected TouchSensor Sensor { get; }
        protected TouchProbe[] Probes { get; }

        public virtual float Measure() => Probes.Max(x => Sensor.Measure(x));
    }
}
