using BepInEx;
using System;

namespace MiscCommands
{

    [BepInPlugin("tech.zinals.plugins.misccommands", "Misc Commands", "1.0.0.0")]
    [BepInDependency("tech.zinals.plugins.chatcommands", BepInDependency.DependencyFlags.HardDependency)]
    public class MiscCommandsMod : BaseUnityPlugin
    {
        internal static BepInEx.Logging.ManualLogSource ModLogger { get; private set; }

        public MiscCommandsMod()
        {
            ModLogger = Logger;

            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
            ChatCommands.ChatSystem.OverwriteAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public void Awake()
        {
            StartCoroutine(Utils.LoadAllItems());
        }

    }
}
