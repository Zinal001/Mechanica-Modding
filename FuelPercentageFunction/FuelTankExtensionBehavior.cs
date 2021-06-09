using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FuelPercentageFunction
{
    public class FuelTankExtensionBehavior : MonoBehaviour
    {
        private Game.FunctionalityFramework.FunctionalObject _ObjectInstance;
        private Game.Fuel.FuelTank _FuelTank;

        private Game.FunctionalityFramework.FObjectVariable _FuelPercentageVar = null;

        public void Initialize(Game.FunctionalityFramework.FunctionalObject objectInstance, Game.Fuel.FuelTank fuelTank)
        {
            _ObjectInstance = objectInstance;
            _FuelTank = fuelTank;
        }

        void Start()
        {
            if (_FuelPercentageVar == null)
                _FuelPercentageVar = new Game.FunctionalityFramework.FObjectVariable("Fuel remaining", _ObjectInstance, new Game.Types.DPercentage(0f), false);
        }

        void OnDestroy()
        {
            if (_ObjectInstance != null)
                _ObjectInstance.objectVariables.Remove(_FuelPercentageVar);

            _FuelPercentageVar.DestroyVariable();
            _FuelPercentageVar = null;
        }

        public void SetFuelPercentageVar(Game.FunctionalityFramework.FObjectVariable objectVariable)
        {
            _FuelPercentageVar = objectVariable;
        }

        public void OnChange(float currentAmount)
        {
            if (_FuelPercentageVar != null)
                _FuelPercentageVar.SetDataValue(currentAmount, true);
        }

    }
}
