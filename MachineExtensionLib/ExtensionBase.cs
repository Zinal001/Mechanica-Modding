using System;
using System.Collections.Generic;
using System.Text;

namespace MachineExtensionLib
{
    public abstract class ExtensionBase
    {
        public abstract String Name { get; }

        protected Game.FunctionalityFramework.FunctionalObject _ObjectInstance { get; private set; }

        public ExtensionBase()
        {
            Configs.Instance.AddExtension(Name);
        }

        internal void SetObjectInstance(Game.FunctionalityFramework.FunctionalObject objectInstance)
        {
            _ObjectInstance = objectInstance;
        }

        public virtual void RunStart(MachineExtensionBehaviour extensionBehaviour, Game.FunctionalityFramework.FunctionalObject objectInstance) { }

        public virtual void RunOnDestroy(MachineExtensionBehaviour extensionBehaviour, Game.FunctionalityFramework.FunctionalObject objectInstance) { }

        public virtual void RunOnUpdate(Game.FunctionalityFramework.FunctionalObject objectInstance) { }

        public virtual void SetVarValue(String variableName, object value) { }

        public virtual void SetVarInstance(String variableName, Game.FunctionalityFramework.FObjectVariable variable) { }
    }
}
