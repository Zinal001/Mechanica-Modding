using BepInEx;
using MachineExtensionLib;
using System;

namespace FurnaceExtensions
{

    [BepInPlugin("tech.zinals.plugins.furnaceextensions", "Furnace Extensions", "1.0.0")]
    public class FurnaceExtensionsMod : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource ModLogger { get; private set; }


        public FurnaceExtensionsMod()
        {
            ModLogger = Logger;
            new Configs(Config);

            MachineExtensionBehaviour.AddExtension<Extensions.CanSmeltExtension>();

            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }
    }
}
