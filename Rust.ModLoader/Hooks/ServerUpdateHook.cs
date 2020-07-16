using System;
using Harmony;
using UnityEngine;

namespace Rust.ModLoader.Hooks
{
    [HarmonyPatch(typeof(ServerMgr), "Update")]
    public class ServerUpdateHook
    {
        [HarmonyPostfix]
        public static void Postfix(ServerMgr __instance)
        {
            try
            {
                ModLoader.Scripts?.Update();
                ModLoader.Scripts?.Invoke("Update");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
