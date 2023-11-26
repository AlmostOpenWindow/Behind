using Doom.GamePlay.Configs;

namespace Configs
{
    public class ConfigID<TConfig> where TConfig : BaseConfig
    {
        public static readonly ConfigID<TConfig> Invalid = new(0);
        private static readonly LoaderConfig _loaderConfig = new();
        
        public readonly int ID;

        public ConfigID(TConfig config)
        {
            if (config is null)
            {
                ID = 0;
                return;
            }
            _loaderConfig.TryRegister(config);
            ID = config.ConfigID;
        }

        internal ConfigID(int id)
        {
            ID = id;
        }

        public static bool operator ==(ConfigID<TConfig> a, ConfigID<TConfig> b)
            => a is not null && b is not null && a.ID == b.ID || (a is null && b is null);

        public static bool operator !=(ConfigID<TConfig> a, ConfigID<TConfig> b)
            => !(a == b);

        public static bool operator ==(TConfig config, ConfigID<TConfig> b)
            => config != null && b is not null && config.ConfigID == b.ID || (b is null && config == null);

        public static bool operator !=(TConfig config, ConfigID<TConfig> b)
            => !(config == b);
        public static bool operator ==(ConfigID<TConfig> b, TConfig config)
            => config != null && b is not null && config.ConfigID == b.ID || (b is null && config == null);

        public static bool operator !=(ConfigID<TConfig> b, TConfig config)
            => !(config == b);

        public static explicit operator ConfigID<TConfig>(TConfig config)
            => new ConfigID<TConfig>(config);
        
        public TConfig Load()
            => _loaderConfig.Load(this);
        
        protected bool Equals(ConfigID<TConfig> other)
        {
            return ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConfigID<TConfig>)obj);
        }

        public override int GetHashCode()
            => ID.GetHashCode();

        public override string ToString()
        {
            var loaded = Load();
            return loaded.name;
        }
    }
}