using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Rust.ModLoader.Scripting
{
    internal static class CSharpCompiler
    {
        private static readonly CSharpCompilationOptions CompilationOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            optimizationLevel: OptimizationLevel.Release);

        private static readonly EmitOptions EmitOptions = new EmitOptions(false, DebugInformationFormat.Embedded);

        private static int _counter = 0;

        public static CompilationResult Build(string name, string code)
        {
            var syntaxTree = ParseCode(code);
            var references = GetReferences();

            var assemblyName = $"{name}.{_counter++}";
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, references, CompilationOptions);

            using (var ms = new MemoryStream())
            {
                var emitResult = compilation.Emit(ms, options: EmitOptions);

                var assemblyData = emitResult.Success
                    ? ms.ToArray()
                    : null;

                var errors = emitResult.Diagnostics
                    .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString())
                    .ToList();

                return new CompilationResult(
                    emitResult.Success,
                    assemblyData,
                    errors);
            }
        }

        private static SyntaxTree ParseCode(string code)
        {
            var sourceText = SourceText.From(code);
            var options = CSharpParseOptions.Default.WithLanguageVersion( LanguageVersion.Latest );
            return CSharpSyntaxTree.ParseText(sourceText, options);
        }

        private static IEnumerable<MetadataReference> GetReferences()
        {
            // TODO: these should probably be cached!
            yield return MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            yield return MetadataReference.CreateFromFile(typeof(RustScript).Assembly.Location);
            yield return MetadataReference.CreateFromFile(typeof(UnityEngine.Debug).Assembly.Location);
            yield return MetadataReference.CreateFromFile(typeof(ServerMgr).Assembly.Location);

            // TODO: more
            // TODO: extract inter-script dependencies from the SyntaxTree?
        }
    }
}
