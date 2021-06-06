using BepInEx;
using System;

namespace MiscCommands
{

    [BepInPlugin("tech.zinals.plugins.misccommands", "Misc Commands", "1.0.0.0")]
    [BepInDependency("tech.zinals.plugins.chatcommands", BepInDependency.DependencyFlags.HardDependency)]
    public class MiscCommandsMod : BaseUnityPlugin
    {

        public MiscCommandsMod()
        {
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
            ChatCommands.ChatSystem.OverwriteAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

    }
}
