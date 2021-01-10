using System;
using Harmony;
using UnityEngine;

namespace Rust.ModLoader.Hooks.Server
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
                ModLoader.Scripts?.Broadcast("Update");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
