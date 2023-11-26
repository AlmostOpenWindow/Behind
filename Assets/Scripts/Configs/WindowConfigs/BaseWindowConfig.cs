using System;
using Windows.WindowControllers.Base;
using GameEntryPoint;
using JetBrains.Annotations;
using Services.Debug;
using UnityEngine;

namespace Configs.WindowConfigs
{
    public abstract class BaseWindowConfig : BaseConfig
    {
        [Tooltip("Префаб окна")]
        public GameObject Prefab;

        [CanBeNull]
        public IWindowController Open(
            ApplicationContainer applicationContainer,
            Action<IWindowController> closeCallback)
        {
            if (Prefab == null)
            {
                var logService = applicationContainer.ServicesEntity.Get<ILogService>();
                logService.Error("[WindowService] Cannot find prefab in WindowConfig \"" + name + "\"");
                return null;
            }
            
            var windowInstance = Instantiate(
                Prefab, 
                applicationContainer.SceneData.GamePlayCanvas.WindowsContainer);
            
            return DoOpen(applicationContainer, windowInstance, closeCallback);
        }

        protected abstract IWindowController DoOpen(
            ApplicationContainer applicationContainer, 
            GameObject windowInstance, 
            Action<IWindowController> closeCallback);
    }
}