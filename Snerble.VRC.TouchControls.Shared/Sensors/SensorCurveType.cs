using Snerble.VRC.TouchControls.Shared.DataAnnotations;
using System.ComponentModel;

namespace Snerble.VRC.TouchControls.Shared.Sensors
{
    /// <summary>
    /// Indicates the type of curve to use for the sensor measurement.
    /// </summary>
    [Options(SensorConstants.CurveKey)]
    public enum SensorCurveType
    {
        /// <summary>
        /// Measurement is applied 1:1.
        /// </summary>
        [Option("d")]
        [Description("Measurement is applied 1:1.")]
        Direct,
        /// <summary>
        /// Measurement is applied as either 0 or 1.
        /// </summary>
        [Option("b")]
        [Description("Measurement is applied as either 0 or 1.")]
        Binary,
        /// <summary>
        /// Measurement is passed through a custom curve before being applied.
        /// </summary>
        [Option("c")]
        [Description("Measurement is passed through a custom curve before being applied.")]
        Custom
    }
}
