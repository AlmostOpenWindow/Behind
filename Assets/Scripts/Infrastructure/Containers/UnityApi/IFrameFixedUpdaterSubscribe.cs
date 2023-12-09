using Components.Common;

namespace Infrastructure.Containers.UnityApi
{
    public interface IFrameFixedUpdaterSubscribe
    {
        void AddFixedUpdater(IFixedUpdater fixedUpdater);
        void RemoveFixedUpdater(IFixedUpdater fixedUpdater);
    }
}