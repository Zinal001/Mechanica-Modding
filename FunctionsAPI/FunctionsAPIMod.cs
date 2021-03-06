using BepInEx;
using System;

namespace FunctionsAPI
{

    [BepInPlugin("tech.zinals.plugins.functionsapi", "Functions API", "1.0.3")]
    public class FunctionsAPIMod : BaseUnityPlugin
    {
        internal static BepInEx.Logging.ManualLogSource ModLogger { get; private set; }

        public FunctionsAPIMod()
        {
            ModLogger = Logger;
            Configs.Initialize(Config);

            AddExampleFunctions();
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }

        private static void AddExampleFunctions()
        {
            FunctionsSystem.Overwrite(new CustomFunction(new ExampleFunctions.FAddition4SlotsCustomFunction(null), "/Math"));
            FunctionsSystem.Overwrite(new CustomFunction(new ExampleFunctions.FSubtraction4SlotsCustomFunction(null), "/Math"));
            FunctionsSystem.Overwrite(new CustomFunction(new ExampleFunctions.ExtendedWaitCustomFunction(null), "/Execution Control"));
        }
    }
}
