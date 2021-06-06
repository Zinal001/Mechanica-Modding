using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatCommands.Commands
{
    public class HelpCommand : ChatCommand
    {
        public override string Name => "Help";
        public override string Command => "help";
        public override string Description => "Display a list of available commands";
        public override string Syntax => "/help <command>";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                ChatCommand[] cmds;

                if (e.Arguments.Any())
                    cmds = ChatSystem.GetCommands().Where(c => c.Command.Contains(e.Arguments[0])).OrderBy(c => c.Command).ToArray();
                else
                    cmds = ChatSystem.GetCommands().OrderBy(c => c.Command).ToArray();

                foreach (ChatCommand cmd in cmds)
                    e.AddResponse($"/{cmd.Command}{(String.IsNullOrEmpty(cmd.Description) ? "" : $" - {cmd.Description}")}");

                e.Handled = true;
            }
        }
    }
}
