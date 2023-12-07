using Components.Common;

namespace Infrastructure.Containers.UnityApi
{
    public interface IFrameLateUpdaterSubscribe
    {
        void AddLateUpdater(ILateUpdater lateUpdater);
        void RemoveLateUpdater(ILateUpdater lateUpdater);
    }
}