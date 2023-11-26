using Components.Common;
using Doom.GamePlay.Components.Common;

namespace Doom.Infrastructure.Containers.UnityApi
{
    public interface IFrameFixedUpdaterSubscribe
    {
        void AddFixedUpdater(IFixedUpdater fixedUpdater);
        void RemoveFixedUpdater(IFixedUpdater fixedUpdater);
    }
}