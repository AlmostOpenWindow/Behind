using Components.Common;
using UnityEngine;

namespace Components.Units
{
    public class Hero : MonoController, IUnit
    {
        [SerializeField] private Transform _transform;


        public Vector3 Position => _transform.position;
    }
}