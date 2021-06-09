using System;
using System.Collections.Generic;
using System.Text;

namespace MachineExtensionLib.Attributes
{
    public class MachineTypeAttribute : Attribute
    {
        public Type MachineType { get; set; }

        public MachineTypeAttribute(Type machineType)
        {
            MachineType = machineType;
        }
    }
}
