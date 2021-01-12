using System;
using Harmony;
using UnityEngine;

namespace Rust.ModLoader.Hooks.Server
{
    [HarmonyPatch(typeof(BasePlayer), "PlayerInit")]
    public class ServerPlayerConnectedHook
    {
        [HarmonyPostfix]
        public static void Postfix(BasePlayer __instance)
        {
            try
            {
                ModLoader.Scripts?.Broadcast("PlayerConnected", __instance);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
