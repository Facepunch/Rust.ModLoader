using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Rust.ModLoader.Exceptions;
using Rust.ModLoader.Scripting;
using UnityEngine;

namespace Rust.ModLoader
{
    internal partial class Script : IEquatable<Script>, IDisposable
    {
        public ScriptManager Manager { get; }
        public string Name { get; }
        public string Path { get; private set; }
        public string SourceCode { get; private set; }
        public Assembly Assembly { get; private set; }
        public RustScript Instance { get; private set; }
        public HashSet<string> SoftDependencies { get; }

        public Script(ScriptManager manager, string name)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SoftDependencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public void Dispose()
        {
            try
            {
                Instance?.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError($"{Instance?.GetType().FullName}::Dispose threw: {e}");
            }

            Instance = null;
            Assembly = null;
        }

        internal void Update(string path)
        {
            string code;

            try
            {
                code = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                throw new ScriptLoadException(Name, $"Failed to load script code at path: {path}", e);
            }

            if (code != SourceCode)
            {
                Compile(path, code);
            }
        }

        private void Compile(string path, string code)
        {
            var result = CSharpCompiler.Build(Name, code);

            if (!result.IsSuccess)
            {
                var errorList = string.Join("\n", result.Errors);
                throw new ScriptLoadException(Name, $"Script compile failed:\n{errorList}");
            }

            Assembly assembly;

            try
            {
                assembly = Assembly.Load(result.AssemblyData);
            }
            catch (Exception e)
            {
                throw new ScriptLoadException(Name, "Failed to load assembly", e);
            }

            Initialize(path, code, assembly);
        }

        private void Initialize(string path, string code, Assembly assembly)
        {
            var type = assembly.GetType(Name);
            if (type == null)
            {
                throw new ScriptLoadException(Name, $"Unable to find class '{Name}' in the compiled script.");
            }

            object instance;

            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new ScriptLoadException(Name, $"Exception thrown in script '{Name}' constructor.", e);
            }
                
            if (!(instance is RustScript scriptInstance))
            {
                throw new ScriptLoadException(Name, $"Script class ({type.FullName}) must derive from RustScript.");
            }

            scriptInstance.Manager = Manager;

            Assembly = assembly;
            Instance = scriptInstance;
            Path = path;
            SourceCode = code;

            SoftDependencies.Clear();
            var allSoftReferences = Manager.PopulateScriptReferences(Instance).ToList();
            SoftDependencies.UnionWith(allSoftReferences);

            try
            {
                Instance.Initialize();
            }
            catch (Exception e)
            {
                ReportError("Initialize", e);
            }
        }

        internal void ReportError(string context, Exception e)
        {
            Debug.LogError($"Script '{Name}' threw in {context}: {e}");
        }

        #region IEquatable

        public bool Equals(Script other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Manager, other.Manager) && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Script)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Manager != null ? Manager.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Script left, Script right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Script left, Script right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
