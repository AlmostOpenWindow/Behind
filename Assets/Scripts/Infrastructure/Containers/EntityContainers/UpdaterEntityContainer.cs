using Components.Common;
using Doom.GamePlay.Components.Common;
using Infrastructure.Containers.UnityApi;

namespace Infrastructure.Containers.EntityContainers
{
    public abstract class UpdaterEntityContainer<TEntity> : BaseEntityContainer<TEntity>
    {
        private readonly IFrameUpdaterSubscribe _frameUpdaterSubscribe;
        private readonly IFrameFixedUpdaterSubscribe _frameFixedUpdaterSubscribe;
        private readonly IFrameLateUpdaterSubscribe _frameLateUpdaterSubscribe;

        protected UpdaterEntityContainer(
            IFrameUpdaterSubscribe frameUpdaterSubscribe, 
            IFrameFixedUpdaterSubscribe frameFixedUpdaterSubscribe,
            IFrameLateUpdaterSubscribe frameLateUpdaterSubscribe)
        {
            _frameUpdaterSubscribe = frameUpdaterSubscribe;
            _frameFixedUpdaterSubscribe = frameFixedUpdaterSubscribe;
            _frameLateUpdaterSubscribe = frameLateUpdaterSubscribe;
        }
        
        protected override void OnAdd<TAddEntity>(TAddEntity addEntity)
        {
            base.OnAdd(addEntity);
            if (addEntity is IUpdater updater)
                _frameUpdaterSubscribe.AddUpdater(updater);
            if (addEntity is IFixedUpdater fixedUpdater) 
                _frameFixedUpdaterSubscribe.AddFixedUpdater(fixedUpdater);
            if (addEntity is ILateUpdater lateUpdater)
                _frameLateUpdaterSubscribe.AddLateUpdater(lateUpdater);
        }
    }
}