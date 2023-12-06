using UnityEngine;
using UnityEngine.Events;

namespace Components.GameEvents
{
    public class GameUnityEvent : UnityEvent<Component, object> { }

    public class GameEventListenerComponent : MonoBehaviour
    {
        public GameEvent GameEvent;

        public GameUnityEvent Response;
        
        private void OnEnable()
        {
            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(Component sender, object data)
        {
            Response.Invoke(sender, data);
        }
    }
}