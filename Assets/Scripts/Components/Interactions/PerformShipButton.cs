using System.Collections;
using System.Collections.Generic;
using GameEvents.Configs;
using UnityEngine;

namespace Components.Interactions
{
    public class PerformShipButton : MonoBehaviour, IInteractable
    {
        public GameObject ButtonAudioSource;
        public GameObject ButtonSpotlight;
        public List<GameObject> Spotlights;

        private float _currentTimeGap;
        private int _spotLightIndex;
        private bool _enabled;
        private bool _routineDone = true;
        private float _currentStartDelay;

        public void Interacted()
        {
            if (ButtonAudioSource != null)
                ButtonAudioSource.SetActive(true);
        }
    }
}