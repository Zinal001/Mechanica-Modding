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
    public class ExtendedIntervalCustomFunction : FImplementation
    {
		private FInput _IntervalTime;
		private FInput _Pause;

		private FOutput _Execute;

		private Coroutine _Loop;

		private int _OverflowMS;

        public ExtendedIntervalCustomFunction(FImplementation _blueprint)
        {
			if (_blueprint == null)
				blueprint = this;
			else
				blueprint = _blueprint;

			FName = "Interval (Extended)";
			FDesc = "Executes the output pin at a regular interval (in seconds) as specified by the 'interval' input.";
			functionalityIcon = EFunctionalityIcon.Time;
			topBarColour = new Color32(60, 68, 80, byte.MaxValue);
			minimumBlockWidth = 200f;
		}

		public ExtendedIntervalCustomFunction(FunctionalObject _ownerObject, FInstance _instance, FImplementation _blueprint) : this(_blueprint)
		{
			SetupInstancedImplementation(_ownerObject, _instance);
		}
		public override FImplementation CreateInstancedCopy(FInstance instance)
		{
			return new CommonFunctionality.FIntervalExecution(blueprint.ownerObject, instance, blueprint);
		}

		protected override void InitializeIO()
		{
			base.InitializeIO();
			_IntervalTime = new FInput("Interval", "The amount of time to wait between executing the start pin and the done pin being executed.", new DNumber(1.0m), new DNumber(0.0m), instance);
			_IntervalTime.EnableIsPin();
			_IntervalTime.EnableIsData(InputUIStyle.NumberInput, null, new Action(this.IntervalTimeSubmitted), null, null);

			_Pause = new FInput("Pause Timer", "Pause the timer", new DBool(false), new DBool(false), instance);
			_Pause.EnableIsPin();

			_Execute = new FOutput("Executed", "This will execute forever with a delay between each execution.", new DExecute(false), false, instance);
		}

		protected void IntervalTimeSubmitted()
		{
			string text = _IntervalTime.pDataInputRect.Find("InputField").GetComponent<InputField>().text;
			decimal num;
			if (decimal.TryParse(text, out num) && text != "0")
			{
				if (num < 0m)
					num = 0m;

				if (decimal.ToSingle(num) > 3.40282347E+38f)
					num = decimal.Parse(float.MaxValue.ToString("#"));

				_IntervalTime.SetDataValue(num, true);
				return;
			}

			_IntervalTime.SetDataValue(0m, true);
		}
		public override void OnInstanceCreated()
		{
			base.OnInstanceCreated();
			_Loop = Singleton<EditMenuSystem>.Instance.StartCoroutine(this.Loop());
		}

		protected System.Collections.IEnumerator Loop()
		{
			while (instance != null && !instance.pIsDestroyed)
			{
				_Pause.GetSourceLinkData();
				if ((bool)_Pause.pData.GetValue())
				{
					yield return new WaitForSeconds(0.1f);
					continue;
				}

				_IntervalTime.GetSourceLinkData();

				float time = decimal.ToSingle((decimal)_IntervalTime.pData.GetValue());
				time = Mathf.Clamp(time, 0f, float.MaxValue);

				if (Time.deltaTime + _OverflowMS / 1000f > Singleton<EditMenuSystem>.Instance.tickTime + time)
				{
					int num = (int)(Singleton<EditMenuSystem>.Instance.tickTime * 1000f) + (int)(time * 1000f);
					int num2 = (int)(Time.deltaTime * 1000f) + _OverflowMS;
					int num3 = num2 / num;
					int num4 = num2 % num;
					for (int i = 0; i < num3; i++)
					{
						_Execute.RunDownStreamExePins();
					}

					_OverflowMS = num4;
				}

				_Execute.RunDownStreamExePins();
				yield return new WaitForSeconds(Singleton<EditMenuSystem>.Instance.tickTime);
				yield return new WaitForSeconds(time);
			}
			yield break;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000B39F8 File Offset: 0x000B1BF8
		public override void OnInstanceDestroyed()
		{
			Singleton<EditMenuSystem>.Instance.StopCoroutine(_Loop);
			base.OnInstanceDestroyed();
		}

	}
}
