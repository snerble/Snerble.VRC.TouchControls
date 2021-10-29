using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Snerble.VRC.TouchControls
{
    public static class HookHelper
    {
        private static MethodInfo[] _hookMethods;

        static HookHelper()
        {
            _hookMethods = typeof(HookHelper)
                .Assembly
                .GetTypes()
                .SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(x => Attribute.IsDefined(x, typeof(HookAttribute)))
                .ToArray();

#if DEBUG
            foreach (var hook in _hookMethods)
            {
                MelonLogger.Msg("Registered hook '{0}' in {1}",
                    hook.Name,
                    hook.DeclaringType);
            }
#endif
        }

        public static void InvokeHooks(object[] args = null, [CallerMemberName] string methodName = null)
        {
            args = args ?? Array.Empty<object>();

            var methods = _hookMethods
                .Where(x => x.Name == methodName)
                .Where(x => x.GetParameters()
                    .Select(p => p.ParameterType)
                    .SequenceEqual(args.Select(a => a?.GetType())));

            foreach (var method in methods)
            {
                try
                {
                    method.Invoke(null, args);
                }
                catch (Exception ex)
                {
                    ex = ex.InnerException ?? ex;
                    MelonLogger.Error("{0}: {1}\n\t{2}", ex.GetType(), ex.Message, ex.StackTrace);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class HookAttribute : Attribute { }
}
