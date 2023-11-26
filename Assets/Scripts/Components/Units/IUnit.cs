using UnityEngine;

namespace Components.Units
{
    public interface IUnit
    {
        Vector3 Position { get; } 
        //Vector3 Direction { get; }
        
        // mb useful in future
        // IMovable Movable { get; }
        // IHealth Health { get; }
        // IParametersStorage ParametersStorage { get; }
        // IEffectHolder EffectHolder { get; }
    }
}