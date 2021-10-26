using MelonLoader;
using Snerble.VRC.TouchControls.Touch;
using VRC.Playables;

namespace Snerble.VRC.TouchControls.Parameters
{
    public class TouchParameterDriver
    {
        private readonly TouchUnit _touch;
        private readonly AvatarParameter _parameter;

        private object _lastValue = null;

        public TouchParameterDriver(TouchUnit touch, AvatarParameter parameter)
        {
            _touch = touch;
            _parameter = parameter;

            Init();
        }

        private void Init()
        {
            // Set the toggle unit to the parameter value
            if (_parameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0 == AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool
                && _touch is ToggleTouchUnit toggleUnit)
            {
                toggleUnit.IsSet = _parameter.prop_Boolean_0;
            }
        }

        public void Update()
        {
            float measurement = _touch.Measure();

            object value;
            switch (_parameter.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0)
            {
                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Bool:
                    value = measurement == 1f;

                    if (value == _lastValue)
                        return;
                    _lastValue = value;

                    _parameter.prop_Boolean_0 = (bool)value;
                    break;

                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Int:
                    value = (int)measurement;

                    if (value == _lastValue)
                        return;
                    _lastValue = value;

                    _parameter.prop_Int32_1 = (int)value;
                    break;

                case AvatarParameter.EnumNPublicSealedvaUnBoInFl5vUnique.Float:
                    value = measurement;

                    if (value == _lastValue)
                        return;
                    _lastValue = value;

                    _parameter.prop_Single_0 = (float)value;
                    break;
                default:
                    return;
            }

        }
    }
}
