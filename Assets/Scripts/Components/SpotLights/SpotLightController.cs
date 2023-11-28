using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpotLightController : MonoBehaviour
{
    public List<GameObject> Spotlights;
    public float TimeGap = 1f;

    private float _currentTimeGap;
    private int _spotLightIndex;
    private bool _enabled;
    private bool _routineDone = true;
    private void Update()
    {
        if (!_routineDone)
            return;
        
        if (!_enabled)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _enabled = true;
                StartCoroutine(EnableAllLightsRoutine());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _enabled = false;
                StartCoroutine(DisableAllLightsRoutine());
            }
        }
    }
    
    public IEnumerator EnableAllLightsRoutine()
    {
        _spotLightIndex = 0;
        _routineDone = false;
        
        while (_spotLightIndex >= 0 && _spotLightIndex != Spotlights.Count)
        {
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

    private IEnumerator DisableAllLightsRoutine()
    {
        _spotLightIndex = Spotlights.Count - 1;
        _routineDone = false;
        
        while (_spotLightIndex >= 0 && _spotLightIndex != Spotlights.Count)
        {
            if (_currentTimeGap > 0f)
            {
                _currentTimeGap -= Time.deltaTime;
                yield return null;
            }
            else
            {
                Spotlights[_spotLightIndex].SetActive(false);
                _spotLightIndex--;
        
                _currentTimeGap += TimeGap;
            }
        }
        
        _routineDone = true;
    }
}
