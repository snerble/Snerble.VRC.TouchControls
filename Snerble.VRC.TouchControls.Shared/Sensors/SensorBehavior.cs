using Snerble.VRC.TouchControls.Shared.DataAnnotations;
using System.ComponentModel;

namespace Snerble.VRC.TouchControls.Shared.Sensors
{
    /// <summary>
    /// Indicates what special behavior should be used for a sensor.
    /// </summary>
    [Options(SensorConstants.BehaviorKey)]
    public enum SensorBehavior : byte
    {
        /// <summary>
        /// Measures touch directly
        /// </summary>
        [Option("n")]
        [Description("Measures touch directly")]
        Normal,
        /// <summary>
        /// Latches and unlatches once threshold is reached.
        /// Output will be max when latched, and normal when unlatched.
        /// </summary>
        [Option("t")]
        [Description("Latches and unlatches once threshold is reached. Output will be max when latched, and normal when unlatched.")]
        Latch
    }
}
