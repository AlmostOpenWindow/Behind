using System.Collections.Generic;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Components.CharacterControllers
{
    public class FlashlightController : MonoBehaviour
    {
        public List<GameObject> lights;
        private bool _isTurnedOn;

        private bool _subscribed;
        private IInputService _input;
        
        public void Construct(IInputService input)
        {
            _input = input;
            _input.Data.FlashlightClickEvent += OnFlashlightClicked;
            _subscribed = true;
        }
        
        public void OnEnable()
        {
            if (_input == null || _subscribed)
                return;
            
            _subscribed = true;
            _input.Data.FlashlightClickEvent += OnFlashlightClicked;
        }

        public void OnDisable()
        {
            if (_input == null || !_subscribed)
                return;
            
            _subscribed = false;
            _input.Data.FlashlightClickEvent -= OnFlashlightClicked;
        }

        private void OnFlashlightClicked()
        {
            _isTurnedOn = !_isTurnedOn;
            TurnOnOffLights(_isTurnedOn);
        }

        private void TurnOnOffLights(bool on)
        {
            foreach (var myLight in lights)
            {
                myLight.SetActive(on);
            }
        }
    }
}