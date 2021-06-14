using Game.FunctionalityFramework;
using Game.Types;
using Game.UI;
using Game.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FunctionsAPI.ExampleFunctions
{
    public class ExtendedWaitCustomFunction : FImplementation
    {
        private FInput _Start;
        private FInput _WaitTime;
        private FInput _Reset;
        private FOutput _Done;

        private List<Coroutine> _Delays = new List<Coroutine>();

        public ExtendedWaitCustomFunction(FImplementation _blueprint)
        {
            if (_blueprint == null)
                blueprint = this;
            else
                blueprint = _blueprint;

            FName = "Wait (Extended)";
            FDesc = "Waits the specified amount of time (in seconds) before executing the 'finished' output.";
            functionalityIcon = EFunctionalityIcon.Time;
            topBarColour = new Color32(60, 68, 80, byte.MaxValue);
            minimumBlockWidth = 200f;
        }

        public ExtendedWaitCustomFunction(FunctionalObject _ownerObject, FInstance _instance, FImplementation _blueprint) : this(_blueprint)
        {
            SetupInstancedImplementation(_ownerObject, _instance);
        }

        public override FImplementation CreateInstancedCopy(FInstance instance)
        {
            return new ExtendedWaitCustomFunction(blueprint.ownerObject, instance, blueprint);
        }

        protected override void InitializeIO()
        {
            base.InitializeIO();
            _Start = new FInput("Start", "Executing this will start the delay.", new DExecute(false), null, instance);
            _Start.EnableIsPin();

            _WaitTime = new FInput("Time to wait", "The amount of time to wait between executing the start pin and the done pin being executed.", new DNumber(1.0m), new DNumber(0m), instance);
            _WaitTime.EnableIsPin();
            _WaitTime.EnableIsData(InputUIStyle.NumberInput, null, NumberSubmit_AlwaysPositive, null, null);

            _Reset = new FInput("Reset Timer", "Should the timer be reset?", new DBool(false), new DBool(false), instance);
            _Reset.EnableIsPin();

            _Done = new FOutput("Finished", "This will execute once the delay is done.", new DExecute(false), false, instance);
        }

        public override void InExecutePinCalled(FInput pin)
        {
            base.InExecutePinCalled(pin);
            _Delays.Add(Singleton<EditMenuSystem>.Instance.StartCoroutine(Delay()));
        }

        public override void OnInstanceDestroyed()
        {
            foreach(var delay in _Delays)
            {
                if (delay != null)
                    Singleton<EditMenuSystem>.Instance.StopCoroutine(delay);
            }

            base.OnInstanceDestroyed();
        }

        protected System.Collections.IEnumerator Delay()
        {
            _WaitTime.GetSourceLinkData();
            float timeToWait = decimal.ToSingle((decimal)_WaitTime.pData.GetValue());
            timeToWait = Mathf.Clamp(timeToWait, 0f, float.MaxValue);

            while(timeToWait > 0f)
            {
                _Reset.GetSourceLinkData();
                if ((bool)_Reset.pData.GetValue())
                    timeToWait = decimal.ToSingle((decimal)_WaitTime.pData.GetValue());

                timeToWait--;
                yield return new WaitForSeconds(1);
            }

            if (instance != null && !instance.pIsDestroyed)
                _Done.RunDownStreamExePins();
        }

        protected void NumberSubmit_AlwaysPositive()
        {
            string text = _WaitTime.pDataInputRect.Find("InputField").GetComponent<InputField>().text;
            decimal num;
            if (decimal.TryParse(text, out num) && text != "0")
            {
                if (num < 0m)
                    num = 0m;

                if (decimal.ToSingle(num) > 3.40282347E+38f)
                    num = decimal.Parse(float.MaxValue.ToString("#"));

                _WaitTime.SetDataValue(num, true);
                return;
            }

            _WaitTime.SetDataValue(0m, true);
        }
    }
}
