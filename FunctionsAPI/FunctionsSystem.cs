using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionsAPI
{
    public static class FunctionsSystem
    {
        private static Dictionary<String, CustomFunction> _Functions = new Dictionary<string, CustomFunction>();

        public static bool Register(CustomFunction customFunction)
        {
            if (_Functions.ContainsKey(customFunction.Function.pFName))
                return false;

            _Functions[customFunction.Function.pFName] = customFunction;
            return true;
        }

        public static void Overwrite(CustomFunction customFunction)
        {
            _Functions[customFunction.Function.pFName] = customFunction;
        }

        internal static CustomFunction[] GetFunctions()
        {
            return _Functions.Values.ToArray();
        }
    }
}
