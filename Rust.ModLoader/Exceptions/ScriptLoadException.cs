using System;

namespace Rust.ModLoader.Exceptions
{
    public class ScriptLoadException : Exception
    {
        public string ScriptName { get; }

        public ScriptLoadException(string scriptName, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ScriptName = scriptName;
        }
    }
}
