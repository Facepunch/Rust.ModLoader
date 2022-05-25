using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Facepunch;
using Harmony;
using Rust.ModLoader.Harmony;
using UnityEngine;

namespace Rust.ModLoader.Hooks.Server
{
    [HarmonyPatch(typeof( ResourceDispenser ), "GiveResourceFromItem" )]
    internal class OnGatherItem : BaseTranspileHook
    {
        //Copy paste into each transpile hook
        static IEnumerable<CodeInstruction> Transpiler( IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase originalMethod )
        {
            return DoTranspile( MethodBase.GetCurrentMethod().DeclaringType, instructions, generator, originalMethod );
        }

        public override bool WeaveHook( CodeInstruction instruction )
        {
            if ( instruction.CheckMethod("GiveItem", typeof(Item) ) == false )
            {
                return false;
            }

            MoveBeforeMethod();

            if ( SearchStoreLocal( SearchDirection.Before, typeof( Item ), out var itemLocal ) == null )
            {
                Debug.LogError( "Couldn't find Item local for OnGatherItem" );
                return false;
            }

            // Player
            LoadParameter( 0 );

            // Item
            LoadLocal( itemLocal.LocalIndex );

            // ResourceDispenser
            LoadThis();

            // Item
            LoadParameter( 1 );

            CallHookMethod( GetType() );

            return true;
        }

        public static bool Hook( BaseEntity entity, Item givenItem, ResourceDispenser dispenser, ItemAmount amount )
        {
            var player = entity as BasePlayer;
            var resourceEntity = dispenser.GetComponent<BaseEntity>();

            var args = Pool.Get<OnGatherItemArgs>();
            args.Entity = resourceEntity;
            args.Player = player;
            args.GivenItem = givenItem;

            ModLoader.Scripts?.Broadcast( "OnGatherItem", args );

            bool result = !args.Cancel;

            Pool.Free( ref args );

            return result;
        }
    }

    public class OnGatherItemArgs
    {
        public BasePlayer Player { get; internal set; }

        public BaseEntity Entity { get; internal set; }

        public Item GivenItem { get; internal set; }

        public bool Cancel { get; internal set; }
    }
}
