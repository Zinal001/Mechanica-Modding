using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatCommands
{
    class Patches
    {
        [HarmonyPatch(typeof(Game.UI.ChatSystem), "ParseCommand")]
        [HarmonyPrefix()]
        static bool Patch_ChatSystem_ParseCommand(String message, Game.UI.ChatSystem __instance)
        {
            message = message.Remove(0, 1);

            String[] parts = message.Split(' ').Select(str => str.ToLower()).ToArray();

            String cmd = parts[0];
            String[] args;
            if (parts.Length > 1)
                args = GetArguments(parts.Skip(1).ToArray());
            else
                args = new String[0];

            ChatCommandEventArgs e = ChatSystem.HandleCommand(cmd, args);

            if (e.Handled)
            {
                foreach (String response in e.ResponseLines)
                    __instance.CreateSystemMessage(response, false);

                return false;
            }

            return true;
        }

        private static String[] GetArguments(String[] parts)
        {
            List<String> arguments = new List<String>();

            bool inQuotes = false;
            List<String> quoteWords = new List<String>();

            for (int i = 0; i < parts.Length; i++)
            {
                if (!inQuotes)
                {
                    if (parts[i].StartsWith("\""))
                    {
                        if (parts[i].EndsWith("\""))
                            arguments.Add(parts[i].Substring(1, parts[i].Length - 2));
                        else
                        {
                            inQuotes = true;
                            quoteWords.Add(parts[i].Substring(1));
                        }
                    }
                    else
                        arguments.Add(parts[i]);
                }
                else
                {
                    if (parts[i].EndsWith("\""))
                    {
                        quoteWords.Add(parts[i].Substring(0, parts[i].Length - 1));
                        inQuotes = false;
                        arguments.Add(String.Join(" ", quoteWords));
                        quoteWords.Clear();
                    }
                    else
                        quoteWords.Add(parts[i]);
                }
            }

            return arguments.ToArray();
        }
    }
}
