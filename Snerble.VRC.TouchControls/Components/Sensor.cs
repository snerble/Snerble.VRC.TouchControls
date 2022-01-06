using Snerble.VRC.TouchControls.Extensions;
using Snerble.VRC.TouchControls.Parameters;
using Snerble.VRC.TouchControls.Parsing;
using Snerble.VRC.TouchControls.Shared;
using Snerble.VRC.TouchControls.Shared.Sensors;
using System;
using System.Linq;
using UnityEngine;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.Components
{
    public sealed class Sensor : ComponentBase
    {
        public float Radius;
        public SensorBehavior Behavior;
        public AnimationCurve Curve;
        public int IntValue = 1;
        public float Threshold;
        public Probe[] Probes;
        public IParameter Parameter;

        internal float? LastMeasurement;

        private bool Latch;

        public Sensor(GameObject gameObject) : base(gameObject) { }

        public DynamicBone DynamicBone => GetComponent<DynamicBone>();

        public override void Awake()
        {
            // Parse the object name
            var args = new ArgumentProvider(GameObject.name);

            if (args.GetArg<string>(0) != SensorConstants.SensorIdentifier)
                throw new Exception("Missing sensor identifier.");

            if (DynamicBone == null)
                throw new Exception($"Missing {typeof(DynamicBone)} component.");
            Radius = DynamicBone.m_Radius;

            Probes = DynamicBone.m_Colliders.ToArray()
                .Where(x => x.name.StartsWith(SensorConstants.ProbeIdentifier))
                .Select(x => new Probe(x.gameObject))
                .ToArray();
            if (!Probes.Any())
                throw new Exception("No probes assigned.");

            if (!(args.GetArg<string>(1) is string parameterName))
                throw new Exception("Missing parameter name.");

            var local = args.HasFlag(SensorConstants.LocalFlag);
            Behavior = args.GetEnumKwarg<SensorBehavior>() ?? default;
            Threshold = Math.Max(float.Epsilon, args.GetKwarg(SensorConstants.ThresholdKey, 0.9f));
            if (args.GetArg<string>(2) is string intValueStr)
                IntValue = int.Parse(intValueStr);

            var curveType = args.GetEnumKwarg<SensorCurveType>();
            switch (curveType)
            {
                default:
                case SensorCurveType.Direct:
                    Curve = CurveConstants.LinearCurve;
                    break;
                case SensorCurveType.Binary:
                    Curve = CurveConstants.BooleanCurve;
                    break;
                case null when args.GetKwarg<string>(SensorConstants.CurveKey) is string base64Curve:
                    Curve = new AnimationCurve(KeyframeSerializer.DeserializeBase64(base64Curve));
                    break;
            }

            Parameter = local
                ? new LocalParameter(parameterName)
                : (IParameter)new Parameter(parameterName);

            Log.Msg("Configured '{0}' with {1} probe(s)", GameObject.name, Probes.Length);
#if DEBUG
            foreach (var probe in Probes)
            {
                Log.Msg("Probe: {0}", probe.GameObject.name);
            }
#endif
        }

        public override void Update()
        {
            float measurement = MeasureRaw();

            // Cull measurements that didnt change
            if (LastMeasurement.HasValue && Mathf.Approximately(LastMeasurement.Value, measurement))
                return;
            LastMeasurement = measurement;

            // Apply modifiers to raw measurement
            measurement = Measure(measurement);

            switch (Behavior)
            {
                case SensorBehavior.Normal:
                    switch (Parameter.Type)
                    {
                        case ParameterType.Int:
                            measurement = (int)(IntValue * measurement);
                            goto default;
                        default:
                            Parameter.Set(measurement);
                            break;
                    }
                    break;

                case SensorBehavior.Latch:
                    if (!Latch && measurement == 1)
                    {
                        Latch = true;
                        if (!IsSet())
                        {
                            Parameter.Locked = true;
                            measurement = 1;
                        }
                        else
                        {
                            Parameter.Locked = false;
                        }
                        goto case SensorBehavior.Normal;
                    }

                    // Latch release
                    else if (Latch && measurement != 1)
                    {
                        Latch = false;
                    }

                    if (IsSet())
                    {
                        measurement = 1;
                    }

                    // Fallback to default behavior
                    goto case SensorBehavior.Normal;
            }
        }

        public float MeasureRaw()
        {
            var sphere = ToSphere();
            return Probes.Max(x => x.Measure(sphere));
        }

        public float Measure(float rawMeasurement)
        {
            // Threshold adjustment
            float range = 1 - Threshold;
            if (range == 0)
                return rawMeasurement == 0 ? 0 : 1;

            return Mathf.Clamp(Curve.Evaluate(Mathf.Min(rawMeasurement / range, 1)), 0, 1);
        }

        public bool IsSet()
        {
            if (!Parameter.Locked)
                return false;
            float paramValue = Parameter.Get();
            switch (Parameter.Type)
            {
                default:
                    return paramValue == 1;
                case ParameterType.Int:
                    return ((int)paramValue) == IntValue;
            }
        }

        public Sphere ToSphere() => new Sphere(Transform.position, Radius * Transform.lossyScale.x);

        public override void Dispose()
        {
            base.Dispose();
            Parameter?.Dispose();
            Parameter = null;

            if (Probes != null)
            {
                foreach (var probe in Probes)
                    probe.Dispose();
                Probes = null;
            }
        }
    }
}
