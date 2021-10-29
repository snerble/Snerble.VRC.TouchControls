using UnityEngine;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.Parameters
{
    public class ParameterDriverSettings
    {
        public const string Identifier = "TouchSensor";
        public const string ToggleFlag = "Toggle";
        public const string ThresholdKey = "threshold";

        public string ParamName { get; set; }

        public int TargetValue { get; set; }

        public bool IsToggle { get; set; }

        public float Threshold { get; set; }

        public static ParameterDriverSettings FromName(string s)
        {
            var args = new ArgumentConfigurationProvider(s);

            // Assert identifier is present
            if (!args.GetArg(0, string.Empty).Equals(Identifier))
                return null;

            // Assert parameter name is present
            if (!(args.GetArg<string>(1) is string parameterName))
            {
                Log.Warning("Failed to bind '{0}': Missing required positional argument #1 ({1})",
                    s,
                    nameof(parameterName));
                return null;
            }

            return new ParameterDriverSettings
            {
                ParamName = parameterName,
                IsToggle = args.HasFlag(ToggleFlag),
                TargetValue = Mathf.Clamp(args.GetArg(2, 1), 0, 255),
                Threshold = Mathf.Clamp(args.GetKwarg("threshold", 0.9f), float.Epsilon, 1)
            };
        }
    }
}
