using BepInEx;
using System;

namespace FuelPercentageFunction
{
    [BepInPlugin("tech.zinals.plugins.fuelpercentage", "Fuel Percentage Function", "1.0.0")]
    public class FuelPercentageMod : BaseUnityPlugin
    {
        internal static BepInEx.Logging.ManualLogSource ModLogger { get; private set; }

        public FuelPercentageMod()
        {
            ModLogger = Logger;
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }
    }
}
