using Components.Interactions;
using UnityEngine;

namespace Infrastructure.Mechanics.Interact
{
    public interface IInteractMechanics : IMechanic
    {
        bool TryInteract(Vector3 origin, Vector3 direction, out IInteractable interacted);
    }
}