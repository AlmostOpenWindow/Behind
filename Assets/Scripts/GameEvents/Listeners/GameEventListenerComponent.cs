using GameEvents.Configs;
using GameEvents.Data;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents.Listeners
{
    public class GameUnityEvent : UnityEvent<GameEventData> { }

    public class GameEventListenerComponent : MonoBehaviour, IGameEventListener
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

        public void TriggerEvent(GameEventData gameEventData)
        {
            Response.Invoke(gameEventData);
        }
    }
}