using Components.Common;
using Doom.GamePlay.Components.Common;

namespace Doom.Infrastructure.Containers.UnityApi
{
    public interface IFrameMonoControllerSubscribe
    {
        void AddMonoController(MonoController monoController);
    }
}