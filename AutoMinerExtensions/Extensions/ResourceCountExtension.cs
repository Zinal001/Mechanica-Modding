using Game.FunctionalityFramework;
using MachineExtensionLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMinerExtensions.Extensions
{
    [MachineExtensionLib.Attributes.MachineType(typeof(Game.ObjectScripts.Autominer))]
    public class ResourceCountExtension : ExtensionBase
    {
        private static System.Reflection.MethodInfo Autominer_GetNaturalResource_Method;
        private static readonly BepInEx.Configuration.ConfigEntry<int> UpdateTimerSetting;

        public override string Name => "Resource Count";

        private FObjectVariable _SourceAmountVar;
        private FObjectVariable _SourceMaxAmountVar;

        private UnityEngine.Coroutine _UpdateCoroutine;

        static ResourceCountExtension()
        {
            Autominer_GetNaturalResource_Method = HarmonyLib.AccessTools.Method(typeof(Game.ObjectScripts.Autominer), "GetNaturalResource");
            UpdateTimerSetting = Configs.Instance.AddSetting("Resource Count", "Update Interval", 5, "The number of seconds between each check for resource.");
        }

        public override void RunStart(MachineExtensionBehaviour extensionBehaviour, FunctionalObject objectInstance)
        {
            if (_SourceAmountVar == null && Configs.Instance.IsEnabled(Name))
                _SourceAmountVar = new FObjectVariable("Resource Amount", objectInstance, new Game.Types.DNumber(0m), false);

            if (_SourceMaxAmountVar == null && Configs.Instance.IsEnabled(Name))
                _SourceMaxAmountVar = new FObjectVariable("Resource Amount Max", objectInstance, new Game.Types.DNumber(0m), false);

            if (Configs.Instance.IsEnabled(Name))
                _UpdateCoroutine = extensionBehaviour.StartCoroutine(CustomUpdate());
        }

        public override void RunOnDestroy(MachineExtensionBehaviour extensionBehaviour, FunctionalObject objectInstance)
        {
            if (_SourceAmountVar != null)
            {
                if (objectInstance != null)
                    objectInstance.objectVariables.Remove(_SourceAmountVar);

                _SourceAmountVar.DestroyVariable();
                _SourceAmountVar = null;
            }

            if (_SourceMaxAmountVar != null)
            {
                if (objectInstance != null)
                    objectInstance.objectVariables.Remove(_SourceMaxAmountVar);

                _SourceMaxAmountVar.DestroyVariable();
                _SourceMaxAmountVar = null;
            }

            if (_UpdateCoroutine != null)
            {
                extensionBehaviour.StopCoroutine(_UpdateCoroutine);
                _UpdateCoroutine = null;
            }
        }

        public override void SetVarValue(string variableName, object value)
        {
            if (_SourceAmountVar != null && variableName == _SourceAmountVar.pVariableName)
                _SourceAmountVar.SetDataValue(value, true);
            else if (_SourceMaxAmountVar != null && variableName == _SourceMaxAmountVar.pVariableName)
                _SourceMaxAmountVar.SetDataValue(value, true);
        }

        private System.Collections.IEnumerator CustomUpdate()
        {
            while(true)
            {
                Game.NaturalResources.NaturalResource nr = (Game.NaturalResources.NaturalResource)Autominer_GetNaturalResource_Method.Invoke(_ObjectInstance, new object[0]);
                if(nr == null)
                {
                    SetVarValue(_SourceAmountVar.pVariableName, 0m);
                    SetVarValue(_SourceMaxAmountVar.pVariableName, 0m);
                }
                else
                {
                    decimal amt = nr.amount;
                    decimal amtMax = nr.pMaxAmount;

                    for(int i = 0; i < nr.neighbourResources.Count; i++)
                    {
                        amt += nr.neighbourResources[i].amount;
                        amtMax += nr.neighbourResources[i].pMaxAmount;
                    }

                    SetVarValue(_SourceAmountVar.pVariableName, amt);
                    SetVarValue(_SourceMaxAmountVar.pVariableName, amtMax);
                }

                int waitFor = UpdateTimerSetting.Value;
                if (waitFor < 1)
                    waitFor = 1;
                yield return new UnityEngine.WaitForSeconds(waitFor);
            }
        }
    }
}
