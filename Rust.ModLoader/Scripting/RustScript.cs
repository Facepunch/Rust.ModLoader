using System;
using Rust.ModLoader;

public abstract class RustScript : IDisposable
{
    internal ScriptManager Manager { get; set; }

    public virtual void Initialize() { }

    public virtual void Dispose() { }

    protected void Broadcast(string methodName) => Manager?.Broadcast(methodName);

    protected void Broadcast<T0>(string methodName, T0 arg0) => Manager?.Broadcast(methodName, arg0);

    protected void Broadcast<T0, T1>(string methodName, T0 arg0, T1 arg1) => Manager?.Broadcast(methodName, arg0, arg1);
}
