using System;
using GameEvents.Configs;
using GameEvents.Data;

namespace GameEvents.Listeners
{
    public class GameEventListener : IGameEventListener
    {
        private readonly GameEvent _gameEvent;
        private readonly Action<GameEventData> _eventAction;

        public GameEventListener(GameEvent gameEvent, Action<GameEventData> eventAction)
        {
            _gameEvent = gameEvent;
            _eventAction = eventAction;
        }

        public void Register()
        {
            _gameEvent.RegisterListener(this);    
        }

        public void Unregister()
        {
            _gameEvent.UnregisterListener(this);
        }
        
        public virtual void TriggerEvent(GameEventData eventData)
        {
            _eventAction?.Invoke(eventData);
        }
    }
}