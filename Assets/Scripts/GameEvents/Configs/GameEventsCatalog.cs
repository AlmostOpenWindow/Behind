using Configs;
using UnityEngine;

namespace GameEvents.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameEvents/GameEventsCatalog")]
    public class GameEventsCatalog : BaseConfig
    {
        public GameEvent InteractEvent;
        public GameEvent DismountEvent;
        public GameEvent PerformShipEvent;
    }
}