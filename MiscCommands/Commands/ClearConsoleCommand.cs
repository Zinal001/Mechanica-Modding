using ChatCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiscCommands.Commands
{
    public class ClearConsoleCommand : ChatCommand
    {
        public override string Name => "Clear Console";
        public override string Command => "clear";
        public override string Description => "Clears the console window of messages.";
        public override string Syntax => "/clear";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                System.Reflection.FieldInfo _ChatSystemMessagesField = typeof(Game.UI.ChatSystem).GetField("messages", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (Game.UI.ChatSystem.InstanceExists)
                {
                    List<Game.UI.ChatMessage> messages = (List<Game.UI.ChatMessage>)_ChatSystemMessagesField.GetValue(Game.UI.ChatSystem.Instance);

                    foreach (var msg in messages)
                        UnityEngine.Object.DestroyImmediate(msg.chatEntry.gameObject);

                    messages.Clear();

                    _ChatSystemMessagesField.SetValue(Game.UI.ChatSystem.Instance, messages);
                    e.AddResponse($"Console cleared");
                }
                else
                    e.AddResponse("Failed to clear console: No chat system instance found.");

                e.Handled = true;
            }
        }
    }
}
