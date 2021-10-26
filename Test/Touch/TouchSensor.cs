namespace Snerble.VRC.TouchControls.Touch
{
    public abstract class TouchSensor
    {
        public abstract float Measure(TouchProbe probe);
    }

    public class AggregateTouchSensor : TouchSensor
    {
        public AggregateTouchSensor(IEnumerable<TouchSensor> sensors) => Sensors = sensors.ToArray();

        public TouchSensor[] Sensors { get; }

        public override float Measure(TouchProbe probe) => Sensors.Max(x => x.Measure(probe));
    }
}
