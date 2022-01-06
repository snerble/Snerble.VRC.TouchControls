using System;

namespace Snerble.VRC.TouchControls.Shared.DataAnnotations
{
    /// <summary>
    /// Indicates what option value represents a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class OptionAttribute : Attribute
    {
        public OptionAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));

            Value = value;
        }

        /// <summary>
        /// Gets the value for the option.
        /// </summary>
        public string Value { get; }
    }
}
