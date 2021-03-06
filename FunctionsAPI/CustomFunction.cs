using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionsAPI
{
    public class CustomFunction
    {
        public Game.FunctionalityFramework.FImplementation Function { get; private set; }
        public String DirectoryPath { get; private set; }
        public bool IsExternallyVisible { get; set; } = false;

        public CustomFunction(Game.FunctionalityFramework.FImplementation function, String directoryPath, bool enabledByDefault = true)
        {
            Function = function;
            DirectoryPath = directoryPath;
            Configs.AddConfig(this, enabledByDefault);
        }
    }
}
