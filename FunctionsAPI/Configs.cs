using BepInEx.Configuration;using Game.FunctionalityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionsAPI
{
    public static class Configs
    {
        internal static ConfigFile _ModConfig;
        private static Dictionary<CustomFunction, ConfigEntry<bool>> _EnabledFunctions = new Dictionary<CustomFunction, ConfigEntry<bool>>();

        public static bool IsEnabled(CustomFunction cf)
        {
            if (!_EnabledFunctions.ContainsKey(cf))
                return false;

            return _EnabledFunctions[cf].Value;
        }

        internal static void AddConfig(CustomFunction cf, bool defaultValue = true)
        {
            if (_EnabledFunctions.ContainsKey(cf))
                _EnabledFunctions[cf].Value = defaultValue;
            else
                _EnabledFunctions[cf] = _ModConfig.Bind(cf.Function.pFName, "Is Enabled", defaultValue, "Is this function enabled?");
        }
    }
}
