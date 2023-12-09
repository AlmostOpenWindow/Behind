using UnityEngine;

namespace GameEvents.Data
{
    public class InteractEventData : GameEventData
    {
        public readonly GameObject Interacted;

        public InteractEventData(GameObject sender, GameObject interactedObj) : base(sender)
        {
            Interacted = interactedObj;
        }
    }
}