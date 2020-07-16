using System.Collections.Generic;

namespace Rust.ModLoader.Scripting
{
    internal class CompilationResult
    {
        public bool IsSuccess { get; }
        public byte[] AssemblyData { get; }
        public List<string> Errors { get; }

        public CompilationResult(bool success, byte[] assembly, List<string> errors)
        {
            IsSuccess = success;
            AssemblyData = assembly;
            Errors = errors;
        }
    }
}
