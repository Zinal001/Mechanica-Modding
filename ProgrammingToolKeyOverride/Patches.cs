using Game.Equipping;
using Game.InventoryFramework;
using Game.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammingToolKeyOverride
{
    public class Patches
    {

        [HarmonyLib.HarmonyPatch(typeof(Game.Settings.SettingsManager), "pKey_UseProgTool", HarmonyLib.MethodType.Getter)]
        [HarmonyLib.HarmonyPrefix()]
        public static bool Patch_SettingsManager_pKey_UseProgTool_Get(Game.Settings.SettingsManager __instance, ref UnityEngine.KeyCode __result)
        {
            __result = ProgrammingToolKeyOverrideMod.overrideKey.Value;
            return false;
        }

        [HarmonyLib.HarmonyPatch(typeof(Game.Interaction.InteractionManager), "CanInteractWhileUsingEquippedItem")]
        [HarmonyLib.HarmonyPrefix()]
        public static bool Patch_InteractionManager_CanInteractWhileUsingEquippedItem(ref bool __result)
        {
            if(Singleton<Inventory>.Instance.equippedItem != null && Singleton<Inventory>.Instance.equippedItem.objectClassType == typeof(ProgrammingTool))
            {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
