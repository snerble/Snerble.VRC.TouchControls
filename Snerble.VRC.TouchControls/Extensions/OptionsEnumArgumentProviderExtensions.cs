using Snerble.VRC.TouchControls.Parsing;
using Snerble.VRC.TouchControls.Shared.DataAnnotations;
using System;
using System.Linq;
using System.Reflection;

namespace Snerble.VRC.TouchControls.Extensions
{
    public static class OptionsEnumArgumentProviderExtensions
    {
        public static TEnum? GetEnumKwarg<TEnum>(this ArgumentProvider arguments) where TEnum : struct
        {
            var options = typeof(TEnum).GetCustomAttribute<OptionsAttribute>();
            if (options == null)
                throw new ArgumentException($"Type '{typeof(TEnum)}' is missing an '{typeof(OptionsAttribute)}'");

            string value = arguments.GetKwarg<string>(options.Key);

            return (TEnum?)typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .SingleOrDefault(x =>
                {
                    var option = x.GetCustomAttribute<OptionAttribute>();
                    return option != null && option.Value == value;
                })
                ?.GetValue(null);
        }
    }
}
