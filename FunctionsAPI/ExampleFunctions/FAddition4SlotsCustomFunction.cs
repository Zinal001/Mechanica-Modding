using Game.FunctionalityFramework;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionsAPI.ExampleFunctions
{
    /// <summary>
    /// An extension of the "Add Numbers" function with 4 inputs instead of 2.
    /// </summary>
    public class FAddition4SlotsCustomFunction : FImplementation
    {
        private static readonly String[] NumberToString = new String[] { "First", "Second", "Third", "Fourth" };

        private FInput _Start;
        private FInput[] _NumberInputs;

        private FOutput _Done;
        private FOutput _Result;

        public FAddition4SlotsCustomFunction(FImplementation _blueprint)
        {
            if (_blueprint == null)
                blueprint = this;
            else
                blueprint = _blueprint;

            FName = "Add Numbers (4)";
            FDesc = "Adds 4 numbers together and provides the result.";
            functionalityIcon = EFunctionalityIcon.Addition;
            topBarColour = new UnityEngine.Color32(0, 170, 245, byte.MaxValue);
            minimumBlockWidth = 200f;
        }

        public FAddition4SlotsCustomFunction(FunctionalObject _ownerObject, FInstance _instance, FImplementation _blueprint) : this(_blueprint)
        {
            SetupInstancedImplementation(_ownerObject, _instance);
        }

        public override FImplementation CreateInstancedCopy(FInstance instance)
        {
            return new FAddition4SlotsCustomFunction(blueprint.ownerObject, instance, blueprint);
        }

        protected override void InitializeIO()
        {
            base.InitializeIO();
            _Start = new FInput("Run", "Executing this will calculate the sum of the four numbers below.", new DExecute(false), null, instance);
            _Start.EnableIsPin();

            _NumberInputs = new FInput[4];

            for(int i = 0; i < _NumberInputs.Length; i++)
            {
                _NumberInputs[i] = new FInput($"{NumberToString[i]} Number", $"The {NumberToString[i].ToLower()} number to be added.", new DNumber(0m), new DNumber(0m), instance);
                _NumberInputs[i].EnableIsPin();
                _NumberInputs[i].EnableIsData(InputUIStyle.NumberInput, null, null, null, null);
            }

            _Done = new FOutput("Done", "This will run once the result is calculated.", new DExecute(false), false, instance);
            _Result = new FOutput("Result", "The sum of the four numbers.", new DNumber(0), false, instance);
        }

        public override void InExecutePinCalled(FInput pin)
        {
            base.InExecutePinCalled(pin);

            foreach (FInput input in _NumberInputs)
                input.GetSourceLinkData();

            decimal result = 0.0m;


            foreach (FInput input in _NumberInputs)
            {
                try
                {
                    result += (decimal)input.pData.GetValue();
                }
                catch { }
            }

            _Result.SetDataValue(result);
            _Done.RunDownStreamExePins();
        }
    }
}
