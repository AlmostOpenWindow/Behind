using Configs.WindowConfigs;
using Services;

namespace Infrastructure.Services.Windows
{
    public interface IWindowService : IService
    {
        bool OpenWindow(BaseWindowConfig windowConfig);
    }
}