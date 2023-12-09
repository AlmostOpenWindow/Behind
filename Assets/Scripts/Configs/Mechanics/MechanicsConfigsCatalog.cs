using UnityEngine;

namespace Configs.Mechanics
{
    [CreateAssetMenu(menuName = "Configs/Mechanics/MechanicsCatalog")]
    public class MechanicsConfigsCatalog : BaseConfig
    {
        public MountMechanicsConfig MountMechanicsConfig;
        public RoutineMechanicsConfig RoutineMechanicsConfig;
        public PerformShipMechanicsConfig PerformShipMechanicsConfig;
        public InteractMechanicsConfig InteractMechanicsConfig;
    }
}