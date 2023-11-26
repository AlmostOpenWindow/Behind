using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Windows.WindowViews.Base
{
    public abstract class WindowView : MonoBehaviour
    {
        public event Action CloseEvent;
        
        [SerializeField] 
        private CanvasGroup _canvasGroup;

        public GameObject WindowInstance => gameObject;
        
        protected virtual void DoOpen()
        {
            
        }

        protected virtual void DoClose()
        {
            
        }
        
        public void Show()
        {
            if (_canvasGroup == null)
                return;
            
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            if (_canvasGroup == null)
                return;
            
            _canvasGroup.alpha = 0f;
        }
        
        public void Close()
        {
            DoClose();
        }

        public void Open()
        {
            DoOpen();
        }
        
        [UsedImplicitly]
        public void ActionClose() => CloseEvent?.Invoke();
        

#if UNITY_EDITOR
        public void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
#endif
    }
}