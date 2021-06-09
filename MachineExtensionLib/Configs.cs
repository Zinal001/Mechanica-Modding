using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineExtensionLib
{
    public class Configs
    {
        public static Configs Instance { get; private set; }

        private ConfigFile _Config;

        private Dictionary<String, ConfigEntry<bool>> _EnabledExtensions = new Dictionary<String, ConfigEntry<bool>>();

        private Dictionary<String, Dictionary<String, object>> _ExtensionConfigs = new Dictionary<String, Dictionary<String, object>>();

        public Configs(ConfigFile config)
        {
            Instance = this;
            _Config = config;
        }

        public ConfigEntry<T> AddSetting<T>(String extensionName, String key, T defaultValue, String description = "")
        {
            if (HasSetting(extensionName, key))
                return (ConfigEntry<T>)_ExtensionConfigs[extensionName][key];

            if (!_ExtensionConfigs.ContainsKey(extensionName))
                _ExtensionConfigs[extensionName] = new Dictionary<string, object>();

            ConfigEntry<T> entry = _Config.Bind(extensionName, key, defaultValue, description);

            _ExtensionConfigs[extensionName][key] = entry;
            return entry;
        }

        public void AddExtension(String extensionName)
        {
            if (_EnabledExtensions.ContainsKey(extensionName))
                return;

            _EnabledExtensions[extensionName] = _Config.Bind(extensionName, "Is Enabled", true, "Is this extension enabled?");
        }

        public bool IsEnabled(String extensionName)
        {
            if (_EnabledExtensions.ContainsKey(extensionName))
                return _EnabledExtensions[extensionName].Value;

            return false;
        }

        public bool HasSetting(String extensionName, String key)
        {
            return _ExtensionConfigs.ContainsKey(extensionName) && _ExtensionConfigs[extensionName].ContainsKey(key);
        }

        public T GetSetting<T>(String extensionName, String key, T defaultValue = default)
        {
            if (HasSetting(extensionName, key))
                return ((ConfigEntry<T>)_ExtensionConfigs[extensionName][key]).Value;

            return defaultValue;
        }
    }
}
