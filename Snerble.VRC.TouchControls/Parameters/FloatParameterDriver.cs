using ParamLib;
using Snerble.VRC.TouchControls.Touch;
using UnityEngine;
using VRC.Playables;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.Parameters
{
    public sealed class FloatParameterDriver : ParameterDriver
    {
        private readonly ParameterDriverSettings _settings;
        private readonly AvatarParameter _avatarParam;
        private readonly FloatBaseParam _param;
        private readonly TouchSensor _sensor;

        private float _lastValue;

        internal FloatParameterDriver(
            ParameterDriverSettings settings,
            AvatarParameter avatarParam,
            DynamicBone d)
        {
            _settings = settings;
            _avatarParam = avatarParam;
            
            _param = new FloatBaseParam(_settings.ParamName);

            if (_settings.IsToggle)
            {
                _sensor = new ToggleTouchSensor(d)
                {
                    Threshold = Mathf.Clamp(_settings.Threshold, float.Epsilon, 1),
                    IsSet = _avatarParam.prop_Single_0 == 1f
                };
            }
            else
            {
                _sensor = new TouchSensor(d);
            }
        }

        public override void Update()
        {
            var measurement = _sensor.Measure();

            float value = _settings.IsToggle
                ? Mathf.Floor(measurement)
                : Mathf.Min(measurement / _settings.Threshold, 1);

            if (value == _lastValue)
                return;
            _lastValue = value;

#if DEBUG
            Log.Msg("Set '{0}' to {1}", _settings.ParamName, value); 
#endif

            _param.ParamValue = value;
        }
    }
}
