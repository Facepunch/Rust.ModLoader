using System;

namespace Rust.ModLoader.Exceptions
{
    public class ScriptInvocationException : Exception
    {
        public ScriptInvocationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
