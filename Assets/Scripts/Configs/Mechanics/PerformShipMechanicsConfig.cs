using UnityEngine;

namespace Configs.Mechanics
{
    [CreateAssetMenu(menuName = "Configs/Mechanics/PerformShipMechanicsConfig")]
    public class PerformShipMechanicsConfig : MechanicsConfig
    {
        public float EnableTimeGap = 1.0f;
        public float EnableDelay = 1.0f;
    }
}