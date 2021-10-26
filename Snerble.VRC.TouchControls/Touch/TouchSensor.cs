using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Touch
{
    public abstract class TouchSensor
    {
        public abstract float Measure(TouchProbe probe);
    }

    public class AggregateTouchSensor : TouchSensor
    {
        private readonly TouchSensor[] _sensors;

        public AggregateTouchSensor(IEnumerable<TouchSensor> sensors) => _sensors = sensors.ToArray();

        public override float Measure(TouchProbe probe) => _sensors.Max(x => x.Measure(probe));
    }
}
