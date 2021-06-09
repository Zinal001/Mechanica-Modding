using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnaceExtensions
{
    public class Patches
    {

        [HarmonyPatch(typeof(Game.ObjectScripts.Furnace), "Start")]
        [HarmonyPostfix()]
        public static void Patch_Furnace_Start(Game.FunctionalityFramework.FunctionalObject __instance)
        {
            var ext = __instance.gameObject.GetComponent<MachineExtensionLib.MachineExtensionBehaviour>();
            if (ext == null)
            {
                ext = __instance.gameObject.AddComponent<MachineExtensionLib.MachineExtensionBehaviour>();
                ext.Setup(__instance.GetType());
            }

            ext.Initialize(__instance);

            for (int i = 0; i < __instance.objectVariables.Count; i++)
            {
                if (__instance.objectVariables[i] == null)
                    continue;

                if (!__instance.objectVariables[i].pCanPlayerEditVariable)
                    ext.SetVariableInstance(__instance.objectVariables[i].pVariableName, __instance.objectVariables[i]);
            }
        }
    }
}
