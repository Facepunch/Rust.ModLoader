using System;
using System.IO;
using UnityEngine;

namespace Rust.ModLoader
{
    public static class ModLoader
    {
        public static ScriptManager Scripts { get; }

        static ModLoader()
        {
            try
            {
                var path = Path.GetFullPath(Path.Combine(Application.dataPath, "../Scripts"));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Scripts = new ScriptManager(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
