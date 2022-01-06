using System;
using System.Collections.Generic;
using System.Linq;

namespace Snerble.VRC.TouchControls.Parsing
{
    public sealed class ArgumentProvider
    {
        private readonly List<string> args = new List<string>();
        private readonly HashSet<string> flags = new HashSet<string>();
        private readonly Dictionary<object, string> kwargs = new Dictionary<object, string>();

        public ArgumentProvider(string s)
        {
            var args = s.SplitArgs().ToList().GetEnumerator();
            while (args.MoveNext())
            {
                var arg = args.Current;

                if (arg.StartsWith("--"))
                {
                    if (arg.Length > 2 && args.MoveNext())
                        kwargs[arg.Substring(2, arg.Length - 2)] = args.Current;
                    continue;
                }

                if (arg.StartsWith("/"))
                {
                    if (arg.Length > 1)
                        flags.Add(arg.Substring(1, arg.Length - 1));
                    continue;
                }

                if (arg.StartsWith("-"))
                {
                    var parts = arg.Split(new[] { '=' }, 2);
                    if (parts.Length == 2 && parts[0].Length > 1)
                        kwargs[parts[0].Substring(1, parts[0].Length - 1)] = parts[1];
                    continue;
                }

                this.args.Add(arg);
            }
        }

        public bool HasFlag(string flag)
        {
            return flags.Any(x => x.Equals(flag, StringComparison.OrdinalIgnoreCase));
        }

        public T GetArg<T>(int index, T fallback = default)
        {
            try
            {
                return (T)Convert.ChangeType(args[index], typeof(T));
            }
            catch
            {
                return fallback;
            }
        }

        public T GetKwarg<T>(string key, T fallback = default)
        {
            try
            {
                return (T)Convert.ChangeType(kwargs[key], typeof(T));
            }
            catch
            {
                return fallback;
            }
        }
    }
}
