using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace MachineExtensionLib
{
    public class MachineExtensionBehaviour : MonoBehaviour
    {
        private Game.FunctionalityFramework.FunctionalObject _ObjectInstance;

        private static List<Type> _LoadedExtensions = new List<Type>();

        private List<ExtensionBase> _Extensions = new List<ExtensionBase>();

        public static void AddExtension<T>() where T : ExtensionBase
        {
            if(typeof(T).GetCustomAttribute<Attributes.MachineTypeAttribute>() == null)
            {
                Debug.LogError($"Type {typeof(T).FullName} is missing a MachineTypeAttribute");
                return;
            }

            _LoadedExtensions.Add(typeof(T));
        }

        public void Setup(Type machineType)
        {
            foreach (Type t in _LoadedExtensions)
            {
                Attributes.MachineTypeAttribute machineTypeAttribute = t.GetCustomAttribute<Attributes.MachineTypeAttribute>();
                if (machineType != machineTypeAttribute.MachineType)
                    continue;

                ExtensionBase ext = (ExtensionBase)Activator.CreateInstance(t);
                _Extensions.Add(ext);
            }
        }

        public void Initialize(Game.FunctionalityFramework.FunctionalObject objectInstance)
        {
            _ObjectInstance = objectInstance;

            foreach (ExtensionBase extension in _Extensions)
                extension.SetObjectInstance(_ObjectInstance);
        }

        void Start()
        {
            foreach (ExtensionBase extension in _Extensions)
            {
                extension.RunStart(this, _ObjectInstance);
            }
        }

        void OnDestroy()
        {
            foreach (ExtensionBase extension in _Extensions)
            {
                extension.RunOnDestroy(this, _ObjectInstance);
            }
        }

        public void SetValue(String extensionName, String valueName, object value)
        {
            foreach (ExtensionBase extension in _Extensions.Where(e => e.Name == extensionName))
                extension.SetVarValue(valueName, value);
        }

        public void SetVariableInstance(String variableName, Game.FunctionalityFramework.FObjectVariable variable)
        {
            foreach (ExtensionBase extension in _Extensions)
                extension.SetVarInstance(variableName, variable);
        }
    }
}
