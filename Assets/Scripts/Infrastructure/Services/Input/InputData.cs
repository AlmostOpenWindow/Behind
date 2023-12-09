using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    [Serializable]
    public class InputData
    {
        public event Action FlashlightClickEvent;
        public event Action InteractionClickEvent;
        
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool flashlight;
        public bool sprint;
        public bool lockRotation;
        public bool analogMovement;
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public void TriggerFlashlightClickEvent()
        {
            FlashlightClickEvent?.Invoke();
        }

        public void TriggerInteractClickEvent()
        {
            InteractionClickEvent?.Invoke();
        }
    }
}