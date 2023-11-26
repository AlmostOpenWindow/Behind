using Components.Common;

namespace Infrastructure.Containers.UnityApi
{
    public interface IFrameUpdaterSubscribe
    {
        void AddUpdater(IUpdater updater);
        void RemoveUpdater(IUpdater updater);
    }
}