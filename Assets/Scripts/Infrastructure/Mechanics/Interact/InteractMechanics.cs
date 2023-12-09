using System;
using Components.Common;
using Components.Interactions;
using Configs.Mechanics;
using UnityEngine;

namespace Infrastructure.Mechanics.Interact
{
    public class InteractMechanics : IInteractMechanics, IUpdater
    {
        private readonly InteractMechanicsConfig _config;
        private IInteractable _lastInteracted = null;
        public bool IsEnable { get; private set; }

        public InteractMechanics(InteractMechanicsConfig config)
        {
            _config = config;
        }

        public bool TryInteract(Vector3 origin, Vector3 direction, out IInteractable interacted)
        {
            interacted = null;
            if (_lastInteracted == null)
            {
                Ray r = new Ray(origin, direction);
                if (Physics.Raycast(r, out RaycastHit hitInfo, _config.InteractRange))
                {
                    _lastInteracted = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                    if (_lastInteracted != null)
                        _lastInteracted.Interacted();
                }
            }

            interacted = _lastInteracted;
            return interacted != null;
        }
        
        public void Enable()
        {
            IsEnable = true;
        }

        public void Disable()
        {
            IsEnable = false;
        }

        public void OnUpdate()
        {
            _lastInteracted = null;
        }
    }
}