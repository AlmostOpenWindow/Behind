using GameEvents.Data;

namespace GameEvents.Listeners
{
    public interface IGameEventListener
    {
        void TriggerEvent(GameEventData eventData);
    }
}