using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionsAPI
{
    public static class FunctionsSystem
    {
        private static Char[] _ArraySeparator = new char[] { '|' };
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


        private static List<String> GetBlacklistedFunctions()
        {
            String[] functions = Configs._BlacklistedFunctions.Value.Split(_ArraySeparator, StringSplitOptions.RemoveEmptyEntries);
            return functions.Select(f => f.Trim()).ToList();
        }

        public static void SetBlacklisted(String functionName, bool isBlacklisted)
        {
            List<String> blacklistedFunctions = GetBlacklistedFunctions();
            if (isBlacklisted)
            {
                if (!Configs._BlacklistedFunctions.Value.Contains(functionName))
                {
                    blacklistedFunctions.Add(functionName);
                    Configs._BlacklistedFunctions.Value = String.Join("|", blacklistedFunctions);
                }
            }
            else
            {
                if (blacklistedFunctions.Remove(functionName))
                    Configs._BlacklistedFunctions.Value = String.Join("|", blacklistedFunctions);
            }
        }

        public static bool IsBlacklisted(String functionName)
        {
            return GetBlacklistedFunctions().Contains(functionName);
        }
    }
}
