using Components.CharacterControllers;
using Components.Common;
using UnityEngine;

namespace Components.Units
{
    public class Hero : PausableMonoController, IUnit
    {
        [SerializeField] private UniversalPersonController _universalPersonController;
        [SerializeField] private Transform _transform;
        
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