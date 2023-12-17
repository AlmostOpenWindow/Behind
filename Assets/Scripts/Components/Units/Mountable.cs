using Components.Common;
using Components.Interactions;
using GameEvents.Configs;
using GameEvents.Data;
using UnityEngine;

namespace Components.Units
{
    public class Mountable : PausableMonoController, IInteractable
    {
        [SerializeField] 
        private Transform _dismountPoint;

        [SerializeField] 
        private GameEvent _dismountEvent;

        [SerializeField] 
        private Transform _cameraRoot;
        
        public Transform DismountPoint => _dismountPoint;
        public bool Mounted { get; private set; }
        public Transform CameraRoot => _cameraRoot;

        public void SetMountState(bool state)
        {
            Mounted = state;
            OnMountStateChanged(state);
        }

        protected virtual void OnMountStateChanged(bool mounted) { }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Mounted)
                return;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                _dismountEvent.Raise(new GameEventData(gameObject));
            }
        }
        
        public void Interacted()
        {
            //todo some light or animation indication
        }
    }
}