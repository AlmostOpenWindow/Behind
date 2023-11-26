using Components.Common;
using Doom.GamePlay.Components.Common;

namespace Doom.Infrastructure.Containers.UnityApi
{
    public interface IFrameUpdaterSubscribe
    {
        void AddUpdater(IUpdater updater);
        void RemoveUpdater(IUpdater updater);
    }
}