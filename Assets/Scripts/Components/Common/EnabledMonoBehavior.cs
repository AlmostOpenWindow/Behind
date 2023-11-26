using System;
using UnityEngine;

namespace Doom.GamePlay.Components.Common
{
    public abstract class EnabledMonoBehavior : MonoBehaviour
    {
        public event Action<EnabledMonoBehavior> EnableEvent;
        public event Action<EnabledMonoBehavior> DisableEvent;

        public void SetEnable()
        {
            gameObject.SetActive(true);
            EnableEvent?.Invoke(this);
            OnEnableSet();
        }

        public void SetDisable()
        {
            gameObject.SetActive(false);
            DisableEvent?.Invoke(this);
            OnDisableSet();
        }

        protected virtual void OnEnableSet()
        {
        }
        
        protected virtual void OnDisableSet()
        {
        }
    }
}