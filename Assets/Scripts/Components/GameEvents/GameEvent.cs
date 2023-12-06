using System.Collections.Generic;
using UnityEngine;

namespace Components.GameEvents
{
    [CreateAssetMenu(menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public List<GameEventListenerComponent> Listeners = new List<GameEventListenerComponent>();

        public void Raise(Component sender, object data)
        {
            foreach (var gameEventListener in Listeners)
            {
                gameEventListener.OnEventRaised(sender, data);
            }
        }

        public void RegisterListener(GameEventListenerComponent listenerComponent)
        {
            if (Listeners.Contains(listenerComponent))
                return;
            
            Listeners.Add(listenerComponent);
        }

        public void UnregisterListener(GameEventListenerComponent listenerComponent)
        {
            if (Listeners.Contains(listenerComponent))
                Listeners.Remove(listenerComponent); 
        }
    }
}