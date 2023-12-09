using System;
using Windows.WindowControllers.Base;
using Infrastructure.GameEntryPoint;
using Services.Debug;
using UnityEngine;
using WindowsViewController.WindowControllers;
using WindowsViewController.WindowViews;

namespace Configs.WindowConfigs
{
    [CreateAssetMenu(fileName = "Test2_WindowConfig", menuName = "Config/Windows/TestWindow2")]
    public class Test2WindowConfig : BaseWindowConfig
    {
        protected override IWindowController DoOpen(ApplicationContainer applicationContainer, GameObject windowInstance, Action<IWindowController> closeCallback)
        {
            var logService = applicationContainer.ServicesEntity.Get<ILogService>();

            var view = TryGetView<Test2WindowView>(windowInstance, applicationContainer);
            if (view == null) 
                return null;
            
            var controller = new Test2WindowController();
            OpenController(controller, view, closeCallback);
            
            return controller;
        }
    }
}