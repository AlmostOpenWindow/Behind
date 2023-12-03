using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;

namespace Components.CharacterControllers.classes
{
    public class Flashlight : MonoBehaviour
    {
        public List<GameObject> lights;
        private StarterAssetsInputs _input;
        private bool _isTurnedOn;

        private bool _subscribed;

        public void Start()
        {
            if (_subscribed)
                return;
            
            if (_input == null)
            {
                var inputObject = GameObject.FindGameObjectWithTag("PlayerInput");
                _input = inputObject.GetComponent<StarterAssetsInputs>();
            }

            _subscribed = true;
            _input.clickFEvent += doOnInputPressed;
        }

        public void OnEnable()
        {
            if (_subscribed)
                return;
            
            if (_input == null)
            {
                var inputObject = GameObject.FindGameObjectWithTag("PlayerInput");
                _input = inputObject.GetComponent<StarterAssetsInputs>();
            }

            _subscribed = true;
            _input.clickFEvent += doOnInputPressed;
        }

        public void OnDisable()
        {
            _subscribed = false;
            _input.clickFEvent -= doOnInputPressed;
        }

        private void doOnInputPressed()
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