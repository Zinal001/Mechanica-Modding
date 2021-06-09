using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPercentageFunction
{
    public class Patches
    {

        [HarmonyPatch(typeof(Game.ObjectScripts.Furnace), "Start")]
        [HarmonyPatch(typeof(Game.ObjectScripts.Flamethrower), "Start")]
        [HarmonyPatch(typeof(Game.ObjectScripts.Generator), "Start")]
        [HarmonyPatch(typeof(Game.ObjectScripts.Incinerator), "Start")]
        [HarmonyPatch(typeof(Game.ObjectScripts.SpeederEngine), "Start")]
        [HarmonyPatch(typeof(Game.ObjectScripts.Flamethrower), "Start")]
        [HarmonyPostfix()]
        public static void Patch_Furnace_Start(Game.FunctionalityFramework.FunctionalObject __instance, Game.Fuel.FuelTank ___fuelTank)
        {
            var ext = __instance.gameObject.GetComponent<FuelTankExtensionBehavior>();
            if (ext == null)
            {
                ext = __instance.gameObject.AddComponent<FuelTankExtensionBehavior>();
                ext.Initialize(__instance, ___fuelTank);
            }

            for (int i = 0; i < __instance.objectVariables.Count; i++)
            {
                if (__instance.objectVariables[i] == null)
                    continue;

                if (!__instance.objectVariables[i].pCanPlayerEditVariable)
                {
                    if (__instance.objectVariables[i].pVariableName == "Fuel remaining")
                        ext.SetFuelPercentageVar(__instance.objectVariables[i]);
                }
            }
        }

        [HarmonyPatch(typeof(Game.Fuel.FuelTank), "Drain")]
        [HarmonyPostfix()]
        public static void Patch_FuelTank_Drain(Game.Fuel.FuelTank __instance, float ___currentFill)
        {
            var ext = __instance.gameObject.GetComponentInParent<FuelTankExtensionBehavior>();
            if (ext != null)
                ext.OnChange(___currentFill);
        }

        [HarmonyPatch(typeof(Game.Fuel.FuelTank), "Fill")]
        [HarmonyPostfix()]
        public static void Patch_FuelTank_Fill(Game.Fuel.FuelTank __instance, float ___currentFill)
        {
            var ext = __instance.gameObject.GetComponentInParent<FuelTankExtensionBehavior>();
            if (ext != null)
                ext.OnChange(___currentFill);
        }



    }
}
