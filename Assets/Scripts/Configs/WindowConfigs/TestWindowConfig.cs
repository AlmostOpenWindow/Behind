using System;
using Windows.WindowControllers.Base;
using Infrastructure.GameEntryPoint;
using Services.Debug;
using UnityEngine;
using WindowsViewController.WindowControllers;
using WindowsViewController.WindowViews;

namespace Configs.WindowConfigs
{
    [CreateAssetMenu(fileName = "Test_WindowConfig", menuName = "Config/Windows/TestWindow")]
    public class TestWindowConfig : BaseWindowConfig
    {
        protected override IWindowController DoOpen(
            ApplicationContainer applicationContainer, 
            GameObject windowInstance, 
            Action<IWindowController> closeCallback)
        {
            var logService = applicationContainer.ServicesEntity.Get<ILogService>();

            var view = TryGetView<TestWindowView>(windowInstance, applicationContainer);
            if (view == null) 
                return null;
            
            var controller = new TestWindowController(logService);
            OpenController(controller, view, closeCallback);
            
            return controller;
        }
    }
}