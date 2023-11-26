using System.Collections.Generic;
using Configs;

namespace Doom.GamePlay.Configs 
{
    public class LoaderConfig
    {
        private Dictionary<int, BaseConfig> _configCache = new ();

        public TConfig Load<TConfig>(ConfigID<TConfig> configID) where TConfig : BaseConfig
        {
            if (_configCache.TryGetValue(configID.ID, out var config))
            {
                return config as TConfig;
            }

            return null;
        }

        public ConfigID<TConfig> TryRegister<TConfig>(TConfig config) where TConfig : BaseConfig //todo: make asset resolver
        {
            if (!_configCache.TryGetValue(config.ConfigID, out var cashedConfig))
                _configCache.Add(config.ConfigID, config);
            
            return new ConfigID<TConfig>(config.ConfigID);
        }
    }
}