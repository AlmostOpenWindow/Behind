using Components.CharacterControllers;
using Components.Common;
using UnityEngine;

namespace Components.Units
{
    public class Ship : PausableMonoController, IUnit
    {
        [SerializeField] private ShipController _shipController;
        public Vector3 Position => transform.position;

        public void DoPause()
        {
            Pause();
        }

        public void DoResume()
        {
            Resume();
        }
    }
}