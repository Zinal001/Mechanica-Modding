using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MiscCommands
{
    [RequireComponent(typeof(CharacterController))]
    public class FlyMovementController : Game.Utilities.Singleton<FlyMovementController>
    {
        private CharacterController controller;
        private Game.Player.PlayerMovement playerMovement;

        private float horizontalAxis = 0f;
        private float verticalAxis = 0f;
        private float deadZone = 0.001f;


        protected override void Awake()
        {
            if (base.gameObject == null || this == null)
            {
                return;
            }
            base.Awake();

            controller = GetComponent<CharacterController>();
            playerMovement = GetComponent<Game.Player.PlayerMovement>();
        }

        private void UpdateHorizontalAxis()
        {
            int num = 0;
            if (Input.GetKey(Game.Utilities.Singleton<Game.Settings.SettingsManager>.Instance.pKey_Left))
                num--;
            if (Input.GetKey(Game.Utilities.Singleton<Game.Settings.SettingsManager>.Instance.pKey_Left))
                num++;

            float num2 = Mathf.MoveTowards(horizontalAxis, num, 6f * Time.deltaTime);
            horizontalAxis = (Mathf.Abs(num2) < deadZone) ? 0f : num2;
        }

        private void UpdateVerticalAxis()
        {
            int num = 0;
            if (Input.GetKey(Game.Utilities.Singleton<Game.Settings.SettingsManager>.Instance.pKey_Back))
                num--;
            if (Input.GetKey(Game.Utilities.Singleton<Game.Settings.SettingsManager>.Instance.pKey_Forward))
                num++;

            float num2 = Mathf.MoveTowards(verticalAxis, num, 6f * Time.deltaTime);
            verticalAxis = (Mathf.Abs(num2) < deadZone) ? 0f : num2;
        }

        protected void Update()
        {
            UpdateHorizontalAxis();
            UpdateVerticalAxis();
        }
    }
}
