using UnityEngine;

namespace GameEvents.Data
{
    public class PerformShipEventData : GameEventData
    {
        public GameObject[] Lights { get; }

        public PerformShipEventData(GameObject sender, GameObject[] lights) : base(sender)
        {
            Lights = lights;
        }
    }
}