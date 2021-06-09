using BepInEx;
using BepInEx.Configuration;
using System;

namespace ProgrammingToolKeyOverride
{
    [BepInPlugin("tech.zinals.plugins.programmingtoolkeyoverride", "Programming Tool Key Override", "1.0.0")]
    public class ProgrammingToolKeyOverrideMod : BaseUnityPlugin
    {


        public static ConfigEntry<UnityEngine.KeyCode> overrideKey;

        public ProgrammingToolKeyOverrideMod()
        {
            overrideKey = Config.Bind("General", "Override Key", UnityEngine.KeyCode.B, "The key that should be used instead of the default interaction key.");

            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }

    }
}
