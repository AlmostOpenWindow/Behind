using Windows.WindowControllers.Base;
using Configs.WindowConfigs;
using GameEntryPoint;
using UnityEngine;

namespace Infrastructure.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly ApplicationContainer _applicationContainer;

        public WindowService(ApplicationContainer applicationContainer)
        {
            _applicationContainer = applicationContainer;
        }

        public bool OpenWindow(BaseWindowConfig windowConfig)
        {
            var windowController = windowConfig.Open(_applicationContainer, CloseWindow);
            if (windowController == null)
                return false;

            return true;
        }

        private void CloseWindow(IWindowController controller)
        {
            controller.View.Close();
            Object.Destroy(controller.View.WindowInstance);
        }
    }
}