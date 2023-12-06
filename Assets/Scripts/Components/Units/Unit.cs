using System;
using Components.Common;
using Configs;
using Configs.Units;
using UnityEngine;

namespace Components.Units
{
    public class Unit : PausableMonoController, IUnit
    {
        public event Action<ConfigID<UnitConfig>, Unit> DeadEvent;
        
        [SerializeField]
        private Transform _transform;

        public Vector3 Position => _transform.position;

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