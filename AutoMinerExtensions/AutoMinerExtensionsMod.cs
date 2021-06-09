using BepInEx;
using MachineExtensionLib;
using System;

namespace AutoMinerExtensions
{
    [BepInPlugin("tech.zinals.plugins.autominerextensions", "AutoMiner Extensions", "1.0.0")]
    public class AutoMinerExtensionsMod : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource ModLogger { get; private set; }

        public AutoMinerExtensionsMod()
        {
            ModLogger = Logger;
            new Configs(Config);

            MachineExtensionBehaviour.AddExtension<Extensions.ResourceCountExtension>();

            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }

    }
}
