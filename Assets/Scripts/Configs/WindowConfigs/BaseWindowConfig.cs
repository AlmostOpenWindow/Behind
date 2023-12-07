using System;
using Windows.WindowControllers.Base;
using Windows.WindowViews.Base;
using Infrastructure.GameEntryPoint;
using Infrastructure.Services.Windows;
using JetBrains.Annotations;
using Services.Debug;
using UnityEngine;

namespace Configs.WindowConfigs
{
    public abstract class BaseWindowConfig : BaseConfig
    {
        [Tooltip("Префаб окна")]
        public GameObject Prefab;
        
        public bool TryOpen(ApplicationContainer applicationContainer)
        {
            var windowService = applicationContainer.ServicesEntity.Get<IWindowService>();
            return windowService.OpenWindow(this);
        }
        
        [CanBeNull]
        public IWindowController Open(
            ApplicationContainer applicationContainer,
            Action<IWindowController> closeCallback)
        {
            if (!CheckPrefab(applicationContainer))
                return null;
             
            var windowInstance = Instantiate(
                Prefab, 
                applicationContainer.SceneData.GamePlayCanvas.WindowsContainer);
            
            return DoOpen(applicationContainer, windowInstance, closeCallback);
        }

        private bool CheckPrefab(ApplicationContainer applicationContainer)
        {
            if (Prefab != null) 
                return true;
            
            var logService = applicationContainer.ServicesEntity.Get<ILogService>();
            logService.Error("[WindowService] Cannot find prefab in WindowConfig \"" + name + "\"");
            return false;

        }

        protected TView TryGetView<TView>(
            GameObject windowInstance, 
            ApplicationContainer applicationContainer) 
            where TView : WindowView
        {
            var logService = applicationContainer.ServicesEntity.Get<ILogService>();
            var view = windowInstance.GetComponent<TView>();

            if (view != null) 
                return view;
            
            Destroy(windowInstance);
            logService.Error("[WindowService] Cannot find window view \"" + typeof(TView) + "\" in prefab \"" + Prefab.name + "\"");
            return null;
        }

        protected void OpenController<TView>(
            WindowController<TView> windowController, 
            TView view, 
            Action<IWindowController> closeCallback) 
            where TView : IWindowView
        {
            windowController.Open(view, () => closeCallback?.Invoke(windowController));
        }
        
        protected abstract IWindowController DoOpen(
            ApplicationContainer applicationContainer, 
            GameObject windowInstance, 
            Action<IWindowController> closeCallback);
    }
}