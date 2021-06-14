using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionsAPI
{
    class Patches
    {
        [HarmonyPatch(typeof(Game.FunctionalityFramework.CommonFunctionality), MethodType.Constructor)]
        [HarmonyPostfix()]
        static void Patch_CommonFunctionality_Constructor(Game.FunctionalityFramework.CommonFunctionality __instance)
        {
            var addFunctionMethod = AccessTools.Method(typeof(Game.FunctionalityFramework.CommonFunctionality), "AddFunction");

            int addedFuncs = 0;
            foreach(CustomFunction cf in FunctionsSystem.GetFunctions())
            {
                addFunctionMethod.Invoke(__instance, new object[] { cf.Function, cf.DirectoryPath.StartsWith("/") ? cf.DirectoryPath : $"/{cf.DirectoryPath}" });

                if (cf.IsExternallyVisible)
                    __instance.externalVisibleFunctions.Add(cf.Function);
                
                addedFuncs++;
            }

            FunctionsAPIMod.ModLogger.LogInfo($"{addedFuncs} custom functions registered");
        }

        [HarmonyPatch(typeof(Game.FunctionalityFramework.CommonFunctionality), "AddFunction")]
        [HarmonyPrefix()]
        static bool Patch_CommonFunctionality_AddFunction(Game.FunctionalityFramework.CommonFunctionality __instance, Game.FunctionalityFramework.FImplementation function)
        {
            return !FunctionsSystem.IsBlacklisted(function.pFName);
        }

    }
}
