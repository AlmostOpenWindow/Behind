using System;
using UnityEngine;

namespace Infrastructure.Mechanics.Routines
{
    public class ActivateGoRoutine : Routine
    {
        private readonly GameObject[] _gameObjects;
        private readonly float _delay;
        private readonly float _timeGap;
        private readonly bool _state; 
        
        private int _index = 0;
        private float _currentDelayTime;
        private float _currentTimeGap;
        
        public ActivateGoRoutine(GameObject[] gameObjects, float delay, float timeGap, bool state, Action onComplete) : base(onComplete)
        {
            _gameObjects = gameObjects;
            _delay = delay;
            _timeGap = timeGap;
            _index = 0;
            _currentDelayTime = delay;
            _timeGap = timeGap;
            _state = state;
        }

        public override bool OnUpdate()
        {
            if (_gameObjects == null)
            {
                Debug.LogError("Empty GO collection in Routine");
                _onComplete?.Invoke();
                return true;
            }

            if (_index >= _gameObjects.Length)
            {
                _onComplete?.Invoke();
                return true;
            }
            
            if (_currentDelayTime > 0)
            {
                _currentDelayTime -= UnityEngine.Time.deltaTime;
                return false;
            }

            if (_currentTimeGap > 0)
            {
                _currentTimeGap -= UnityEngine.Time.deltaTime;
                return false;
            }

            if (_index < _gameObjects.Length)
            {
                _gameObjects[_index].SetActive(_state);
                _index++;
                _currentTimeGap += _timeGap;
                return false;
            }

            _onComplete?.Invoke();
            return true;
        }
    }
}