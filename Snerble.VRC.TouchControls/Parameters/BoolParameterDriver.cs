using ParamLib;
using Snerble.VRC.TouchControls.Touch;
using UnityEngine;
using VRC.Playables;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.Parameters
{
    public sealed class BoolParameterDriver : ParameterDriver
    {
        private readonly ParameterDriverSettings _settings;
        private readonly AvatarParameter _avatarParam;
        private readonly BoolBaseParam _param;
        private readonly TouchSensor _sensor;

        private bool _lastValue;

        internal BoolParameterDriver(
            ParameterDriverSettings settings,
            AvatarParameter avatarParam,
            DynamicBone d)
        {
            _settings = settings;
            _avatarParam = avatarParam;

            _param = new BoolBaseParam(_settings.ParamName);

            if (_settings.IsToggle)
            {
                _sensor = new ToggleTouchSensor(d)
                {
                    Threshold = Mathf.Clamp(_settings.Threshold, float.Epsilon, 1),
                    IsSet = _avatarParam.prop_Boolean_0
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

            bool value = _settings.IsToggle
                ? measurement == 1f
                : measurement >= _settings.Threshold;

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
