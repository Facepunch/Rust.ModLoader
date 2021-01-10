using Rust.ModLoader.Scripting;
using System;
using Rust.ModLoader.Exceptions;

namespace Rust.ModLoader
{
    internal partial class Script : IScriptReference
    {
        public void InvokeProcedure(string methodName)
        {
            if (Instance == null)
            {
                return;
            }

            try
            {
                ScriptInvoker.Procedure(Instance, methodName);
            }
            catch (Exception e)
            {
                ReportError($"InvokeProcedure('{methodName}')", e);
            }
        }

        public T0 InvokeFunction<T0>(string methodName)
        {
            if (Instance == null)
            {
                throw new ScriptInvocationException($"Script '{Name}' is not initialized.");
            }

            try
            {
                return ScriptInvoker<T0>.Function(Instance, methodName);
            }
            catch (Exception e)
            {
                throw new ScriptInvocationException($"Function '{Name}::{methodName}()' threw an exception.", e);
            }
        }

        public void InvokeProcedure<T0>(string methodName, T0 arg0)
        {
            if (Instance == null)
            {
                return;
            }
                    
            try
            {
                ScriptInvoker<T0>.Procedure(Instance, methodName, arg0);
            }
            catch (Exception e)
            {
                ReportError($"InvokeProcedure('{methodName}', {typeof(T0).FullName})", e);
            }
        }
        
        public T1 InvokeFunction<T0, T1>(string methodName, T0 arg0)
        {
            if (Instance == null)
            {
                throw new ScriptInvocationException($"Script '{Name}' is not initialized.");
            }

            try
            {
                return ScriptInvoker<T0, T1>.Function(Instance, methodName, arg0);
            }
            catch (Exception e)
            {
                throw new ScriptInvocationException($"Function '{Name}::{methodName}({typeof(T0).FullName})' threw an exception.", e);
            }
        }

        public void InvokeProcedure<T0, T1>(string methodName, T0 arg0, T1 arg1)
        {
            if (Instance == null)
            {
                return;
            }

            try
            {
                ScriptInvoker<T0, T1>.Procedure(Instance, methodName, arg0, arg1);
            }
            catch (Exception e)
            {
                ReportError($"InvokeProcedure('{methodName}', {typeof(T0).FullName}, {typeof(T1).FullName})", e);
            }
        }
    }
}
