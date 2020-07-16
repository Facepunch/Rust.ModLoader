using System;
using System.Linq;
using System.Reflection;

namespace Rust.ModLoader.Scripting
{
    internal static class InvokeBuilder
    {
        public static T GetInvoker<T>(RustScript instance, string methodName) where T : Delegate
        {
            var objectType = instance.GetType();
            var delegateType = typeof(T);

            var signature = delegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance);
            if (signature == null)
            {
                return null;
            }

            var parameterTypes = signature.GetParameters().Select(p => p.ParameterType).ToArray();
            var method = objectType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parameterTypes, null);
            if (method == null || method.DeclaringType == typeof(RustScript))
            {
                return null;
            }

            return (T)method.CreateDelegate(delegateType, instance);
        }
    }
}
