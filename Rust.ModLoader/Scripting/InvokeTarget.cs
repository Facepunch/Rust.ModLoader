using System;
using System.Runtime.CompilerServices;

namespace Rust.ModLoader.Scripting
{
    public readonly struct InvokeTarget : IEquatable<InvokeTarget>
    {
        public RustScript Script { get; }
        public string Method { get; }

        public InvokeTarget(RustScript script, string method)
        {
            Script = script ?? throw new ArgumentNullException(nameof(script));
            Method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public bool Equals(InvokeTarget other)
        {
            return ReferenceEquals(Script, other.Script) && Method == other.Method;
        }

        public override bool Equals(object obj)
        {
            return obj is InvokeTarget other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RuntimeHelpers.GetHashCode(Script) * 397) ^ Method.GetHashCode();
            }
        }

        public static bool operator ==(InvokeTarget left, InvokeTarget right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InvokeTarget left, InvokeTarget right)
        {
            return !left.Equals(right);
        }
    }
}
