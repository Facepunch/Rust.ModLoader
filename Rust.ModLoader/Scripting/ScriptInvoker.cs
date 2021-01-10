using System;

namespace Rust.ModLoader.Scripting
{
    internal static class ScriptInvoker
    {
        private static readonly MruDictionary<InvokeTarget, Action> _procedureCache =
            new MruDictionary<InvokeTarget, Action>(250);

        public static void Procedure(RustScript script, string method)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            Action invoker;
            lock (_procedureCache)
            {
                var target = new InvokeTarget(script, method);
                if (!_procedureCache.TryGetValue(target, out invoker))
                {
                    invoker = InvokeBuilder.GetInvoker<Action>(script, method);
                    _procedureCache.Add(target, invoker);
                }
            }

            invoker?.Invoke();
        }
    }

    internal static class ScriptInvoker<T0>
    {
        private static readonly MruDictionary<InvokeTarget, Action<T0>> _procedureCache =
            new MruDictionary<InvokeTarget, Action<T0>>(100);

        private static readonly MruDictionary<InvokeTarget, Func<T0>> _functionCache =
            new MruDictionary<InvokeTarget, Func<T0>>(250);

        public static void Procedure(RustScript script, string method, T0 arg0)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));
            
            Action<T0> invoker;
            lock (_procedureCache)
            {
                var target = new InvokeTarget(script, method);
                if (!_procedureCache.TryGetValue(target, out invoker))
                {
                    invoker = InvokeBuilder.GetInvoker<Action<T0>>(script, method);
                    _procedureCache.Add(target, invoker);
                }
            }

            invoker?.Invoke(arg0);
        }

        public static T0 Function(RustScript script, string method)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));
            
            Func<T0> invoker;
            lock (_functionCache)
            {
                var target = new InvokeTarget(script, method);
                if (!_functionCache.TryGetValue(target, out invoker))
                {
                    invoker = InvokeBuilder.GetInvoker<Func<T0>>(script, method);
                    _functionCache.Add(target, invoker);
                }
            }

            return invoker != null ? invoker() : default;
        }
    }

    internal static class ScriptInvoker<T0, T1>
    {
        private static readonly MruDictionary<InvokeTarget, Action<T0, T1>> _procedureCache =
            new MruDictionary<InvokeTarget, Action<T0, T1>>(50);

        private static readonly MruDictionary<InvokeTarget, Func<T0, T1>> _functionCache =
            new MruDictionary<InvokeTarget, Func<T0, T1>>(100);

        public static void Procedure(RustScript script, string method, T0 arg0, T1 arg1)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            Action<T0, T1> invoker;
            lock (_procedureCache)
            {
                var target = new InvokeTarget(script, method);
                if (!_procedureCache.TryGetValue(target, out invoker))
                {
                    invoker = InvokeBuilder.GetInvoker<Action<T0, T1>>(script, method);
                    _procedureCache.Add(target, invoker);
                }
            }

            invoker?.Invoke(arg0, arg1);
        }

        public static T1 Function(RustScript script, string method, T0 arg0)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));
            
            Func<T0, T1> invoker;
            lock (_functionCache)
            {
                var target = new InvokeTarget(script, method);
                if (!_functionCache.TryGetValue(target, out invoker))
                {
                    invoker = InvokeBuilder.GetInvoker<Func<T0, T1>>(script, method);
                    _functionCache.Add(target, invoker);
                }
            }

            return invoker != null ? invoker(arg0) : default;
        }
    }
}
