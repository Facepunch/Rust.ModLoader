using System;

namespace Rust.ModLoader.Scripting
{
    internal static class ScriptInvoker
    {
        private static readonly MruDictionary<InvokeTarget, Action> _invokeCache =
            new MruDictionary<InvokeTarget, Action>(250);

        public static void Invoke(RustScript script, string method)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            var target = new InvokeTarget(script, method);
            if (!_invokeCache.TryGetValue(target, out var invoker))
            {
                invoker = InvokeBuilder.GetInvoker<Action>(script, method);
                _invokeCache.Add(target, invoker);
            }

            invoker?.Invoke();
        }
    }

    internal static class ScriptInvoker<T0>
    {
        private static readonly MruDictionary<InvokeTarget, Action<T0>> _invokeCache =
            new MruDictionary<InvokeTarget, Action<T0>>(100);

        public static void Invoke(RustScript script, string method, T0 arg0)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            var target = new InvokeTarget(script, method);
            if (!_invokeCache.TryGetValue(target, out var invoker))
            {
                invoker = InvokeBuilder.GetInvoker<Action<T0>>(script, method);
                _invokeCache.Add(target, invoker);
            }

            invoker?.Invoke(arg0);
        }
    }

    internal static class ScriptInvoker<T0, T1>
    {
        private static readonly MruDictionary<InvokeTarget, Action<T0, T1>> _invokeCache =
            new MruDictionary<InvokeTarget, Action<T0, T1>>(50);

        public static void Invoke(RustScript script, string method, T0 arg0, T1 arg1)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            var target = new InvokeTarget(script, method);
            if (!_invokeCache.TryGetValue(target, out var invoker))
            {
                invoker = InvokeBuilder.GetInvoker<Action<T0, T1>>(script, method);
                _invokeCache.Add(target, invoker);
            }

            invoker?.Invoke(arg0, arg1);
        }
    }
}
