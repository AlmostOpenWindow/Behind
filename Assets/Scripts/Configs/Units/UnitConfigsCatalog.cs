using UnityEngine;

namespace Configs.Units
{
    [CreateAssetMenu(menuName = "Configs/Units/UnitConfigsCatalog")]
    public class UnitConfigsCatalog : BaseConfig
    {
        public HeroConfig HeroConfig;
        public ShipConfig ShipConfig;
    }
}