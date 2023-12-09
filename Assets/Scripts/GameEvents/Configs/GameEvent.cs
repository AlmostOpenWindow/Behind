using System.Collections.Generic;
using Configs;
using GameEvents.Data;
using GameEvents.Listeners;
using UnityEngine;

namespace GameEvents.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameEvents/GameEvent")]
    public class GameEvent : BaseConfig
    {
        private List<IGameEventListener> _listeners = new();
        private Stack<IGameEventListener> _listenersAdditional = new();
        private Stack<IGameEventListener> _listenersRemovable = new();

        public void Raise(GameEventData eventData)
        {
            while (_listenersAdditional.Count > 0)
            {
                _listeners.Add(_listenersAdditional.Pop());
            }

            while (_listenersRemovable.Count > 0)
            {
                _listeners.Remove(_listenersRemovable.Pop());
            }
            
            foreach (var gameEventListener in _listeners)
            {
                gameEventListener.TriggerEvent(eventData);
            }
        }
            
        public void RegisterListener(IGameEventListener listener)
        {
            if (_listenersAdditional.Contains(listener))
                return;
            
            _listenersAdditional.Push(listener);
        }

        public void UnregisterListener(IGameEventListener listener)
        {
            if (_listenersRemovable.Contains(listener))
                _listenersRemovable.Push(listener);
        }
    }
}