using ChatCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiscCommands.Commands
{
    public class FlyCommand : ChatCommand
    {
        public override string Name => "Fly";

        public override string Command => "fly";
        public override string Description => "Let's you fly";
        public override string Syntax => "/fly";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                System.Reflection.FieldInfo _PlayerMovementGravityMultiplierField = typeof(Game.Player.PlayerMovement).GetField("gravityMultiplier", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (Game.Player.PlayerMovement.InstanceExists)
                {
                    if (Game.Player.PlayerControl.InstanceExists && Game.Player.PlayerControl.Instance.isInVehicle)
                        e.AddResponse("No flying in vehicles!");
                    else
                    {
                        if (!Patches._PlayerIsFlying)
                        {
                            Patches._PlayerIsFlying = true;
                            _PlayerMovementGravityMultiplierField.SetValue(Game.Player.PlayerMovement.Instance, 0f);
                            e.AddResponse("Fly mode: enabled");
                        }
                        else
                        {
                            Patches._PlayerIsFlying = false;
                            _PlayerMovementGravityMultiplierField.SetValue(Game.Player.PlayerMovement.Instance, 1f);

                            e.AddResponse("Fly mode: disabled");
                        }
                    }
                }
                else
                    e.AddResponse("Failed to toggle flymode: No player movement system instance found.");

                e.Handled = true;
            }
        }
    }
}
