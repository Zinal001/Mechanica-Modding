using ChatCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiscCommands.Commands
{
    public class KillCommand : ChatCommand
    {
        public override string Name => "Kill";
        public override string Command => "kill";
        public override string Description => "Simple way to end yourself.";
        public override string Syntax => "/kill";


        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                if (Game.Player.PlayerHealth.InstanceExists)
                {
                    Game.Player.PlayerHealth.Instance.ChangeHealth(-Game.Player.PlayerHealth.Instance.currentHealth);
                    e.AddResponse("Suicide!");
                }
                else
                    e.AddResponse("Failed to kill yourself: No player instance found.");

                e.Handled = true;
            }
        }
    }
}
