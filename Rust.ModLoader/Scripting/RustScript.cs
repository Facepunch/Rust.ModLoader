using System;
using Rust.ModLoader;

public abstract class RustScript : IDisposable
{
    internal ScriptManager Manager;

    public virtual void Dispose() { }

    protected void Invoke(string methodName) => Manager?.Invoke(methodName);

    protected void Invoke<T0>(string methodName, T0 arg0) => Manager?.Invoke(methodName, arg0);

    protected void Invoke<T0, T1>(string methodName, T0 arg0, T1 arg1) => Manager?.Invoke(methodName, arg0, arg1);
}
