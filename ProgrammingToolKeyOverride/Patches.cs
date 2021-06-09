﻿using System;
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
    }
}
