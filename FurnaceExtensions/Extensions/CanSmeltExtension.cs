using Game.FunctionalityFramework;
using MachineExtensionLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurnaceExtensions.Extensions
{
    [MachineExtensionLib.Attributes.MachineType(typeof(Game.ObjectScripts.Furnace))]
    public class CanSmeltExtension : ExtensionBase
    {
        private static System.Reflection.MethodInfo _GetSuppliedMaterials_Method;
        private static String[] ValidOres = new string[] { "Components/Wood/Wood", "Components/CopperOre/CopperOre", "Components/IronOre/IronOre", "Components/LithiumOre/LithiumOre", "Components/Sand/Sand" };

        public override string Name => "Can Smelt";

        private FObjectVariable _CanSmeltVar = null;

        private UnityEngine.Coroutine _CheckCanSmeltCoroutine;

        private static readonly BepInEx.Configuration.ConfigEntry<int> UpdateTimerSetting;

        static CanSmeltExtension()
        {
            _GetSuppliedMaterials_Method = HarmonyLib.AccessTools.Method(typeof(Game.ObjectScripts.Furnace), "GetSuppliedMaterials");

            UpdateTimerSetting = Configs.Instance.AddSetting("Can Smelt", "Update Interval", 5, "The number of seconds between each check for materials.");
        }

        public override void RunStart(MachineExtensionBehaviour extensionBehaviour, FunctionalObject objectInstance)
        {
            if (_CanSmeltVar == null && Configs.Instance.IsEnabled(Name))
            {
                _CanSmeltVar = new FObjectVariable("Can smelt", objectInstance, new Game.Types.DBool(false), false);
            }

            if (Configs.Instance.IsEnabled(Name))
            {
                _CheckCanSmeltCoroutine = extensionBehaviour.StartCoroutine(CustomUpdate());
            }
        }

        public override void SetVarInstance(string variableName, FObjectVariable variable)
        {
            if (variableName == "Can smelt")
                _CanSmeltVar = variable;
        }

        public override void RunOnDestroy(MachineExtensionBehaviour extensionBehaviour, FunctionalObject objectInstance)
        {
            if (_CanSmeltVar != null)
            {
                if (objectInstance != null)
                    objectInstance.objectVariables.Remove(_CanSmeltVar);

                _CanSmeltVar.DestroyVariable();
                _CanSmeltVar = null;
            }

            if (_CheckCanSmeltCoroutine != null)
            {
                extensionBehaviour.StopCoroutine(_CheckCanSmeltCoroutine);
                _CheckCanSmeltCoroutine = null;
            }
        }

        public override void SetVarValue(string variableName, object value)
        {
            if (variableName == "Can smelt" && _CanSmeltVar != null)
            {
                _CanSmeltVar.SetDataValue(value, true);
            }
        }

        private System.Collections.IEnumerator CustomUpdate()
        {
            while(true)
            {
                Dictionary<String, int> suppliedMaterials = (Dictionary<String, int>)_GetSuppliedMaterials_Method.Invoke(_ObjectInstance, new object[0]);

                bool canSmelt = false;

                foreach(var pair in suppliedMaterials)
                {
                    if(ValidOres.Contains(pair.Key) && pair.Value >= 2)
                    {
                        canSmelt = true;
                        break;
                    }
                }

                SetVarValue("Can smelt", canSmelt);

                int waitFor = UpdateTimerSetting.Value;
                if (waitFor < 1)
                    waitFor = 1;
                yield return new UnityEngine.WaitForSeconds(waitFor);
            }
        }
    }
}
