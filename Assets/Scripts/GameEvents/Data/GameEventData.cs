using System;
using UnityEngine;

namespace GameEvents.Data
{
    [Serializable]
    public class GameEventData
    {
        public readonly GameObject Sender;
        
        public GameEventData(GameObject sender)
        {
            Sender = sender;
        }
    }
}