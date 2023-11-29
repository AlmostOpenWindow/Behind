using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class LightsButton : MonoBehaviour, IInteractable
{
    public GameObject ButtonAudioSource;
    public List<GameObject> Spotlights;
    public float TimeGap = 1f;
    public float StartDelay = 1f;

    private float _currentTimeGap;
    private int _spotLightIndex;
    private bool _enabled;
    private bool _routineDone = true;
    private float _currentStartDelay;
        
    public void Interact()
    {
        if (ButtonAudioSource != null)
            ButtonAudioSource.SetActive(true);
        
        if (!_routineDone)
            return;
        
        if (!_enabled)
        {
            _enabled = true;
            StartCoroutine(EnableAllLightsRoutine());
        }
    }
    
    private IEnumerator EnableAllLightsRoutine()
    {
        _spotLightIndex = 0;
        _currentStartDelay = StartDelay;
        _routineDone = false;
        
        while (_spotLightIndex >= 0 && _spotLightIndex != Spotlights.Count)
        {
            if (_currentStartDelay > 0f)
            {
                _currentStartDelay -= Time.deltaTime;
                yield return null;
            }
            else
            if (_currentTimeGap > 0f)
            {
                _currentTimeGap -= Time.deltaTime;
                yield return null;
            }
            else
            {
                Spotlights[_spotLightIndex].SetActive(true);
                _spotLightIndex++;
        
                _currentTimeGap += TimeGap;
            }
        }
        
        _routineDone = true;
    }
}