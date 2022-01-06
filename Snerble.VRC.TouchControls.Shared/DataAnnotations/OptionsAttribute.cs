using System;

namespace Snerble.VRC.TouchControls.Shared.DataAnnotations
{
    /// <summary>
    /// Indicates what key is used to identify arguments of a specific enum type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public sealed class OptionsAttribute : Attribute
    {
        public OptionsAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));

            Key = key;
        }

        /// <summary>
        /// Gets the key that identifies the enum.
        /// </summary>
        public string Key { get; }
    }
}
