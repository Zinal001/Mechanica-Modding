using BepInEx;
using System;

namespace ChatCommands
{

    [BepInPlugin("tech.zinals.plugins.chatcommands", "Chat Commands", "1.0.0.0")]
    public class ChatCommandMod : BaseUnityPlugin
    {

        public ChatCommandMod()
        {
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patches));
        }
    }
}
