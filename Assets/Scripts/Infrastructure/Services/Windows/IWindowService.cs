using Configs.WindowConfigs;

namespace Services.Windows
{
    public interface IWindowService : IService
    {
        public bool OpenWindow(BaseWindowConfig windowConfig);
    }
}