using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiscCommands
{
    class Patches
    {
        internal static bool _PlayerIsFlying = false;

        [HarmonyPatch(typeof(Game.Player.PlayerMovement), "MovePlayer")]
        [HarmonyPrefix()]
        static bool Patch_PlayerMovement_MovePlayer(ref Game.Player.PlayerMovement __instance, ref float ___gravityMultiplier, float ___walkSpeed, ref UnityEngine.Vector3 ___movement, Game.Player.PlayerControl ___playerControl)
        {
            if(_PlayerIsFlying)
            {
                ___movement.y = 0f;

                if(!___playerControl.pPlayerHealth.isDead && __instance.canMove && !__instance.movementConstricted && !___playerControl.pIsTypingInputField)
                {
                    if (UnityEngine.Input.GetKey(Game.Utilities.Singleton<Game.Settings.SettingsManager>.Instance.pKey_Jump))
                        ___movement.y = ___walkSpeed * (Game.Utilities.Singleton<Game.GameManager>.InstanceExists ? Game.Utilities.Singleton<Game.GameManager>.Instance.playerSpeedMultiplier : 1f);
                    else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftControl))
                        ___movement.y = -___walkSpeed * (Game.Utilities.Singleton<Game.GameManager>.InstanceExists ? Game.Utilities.Singleton<Game.GameManager>.Instance.playerSpeedMultiplier : 1f);
                }

                if(___playerControl.isInVehicle)
                {
                    _PlayerIsFlying = false;
                    ___gravityMultiplier = 1f;

                    if(Game.UI.ChatSystem.InstanceExists)
                        Game.UI.ChatSystem.Instance.CreateSystemMessage("Flymode disabled: No flying in vehicles!", false);
                }
                else if(___playerControl.pPlayerHealth.isDead)
                {
                    _PlayerIsFlying = false;
                    ___gravityMultiplier = 1f;

                    if (Game.UI.ChatSystem.InstanceExists)
                        Game.UI.ChatSystem.Instance.CreateSystemMessage("Flymode disabled: Death.", false);
                }
            }

            return true;
        }
    }
}
