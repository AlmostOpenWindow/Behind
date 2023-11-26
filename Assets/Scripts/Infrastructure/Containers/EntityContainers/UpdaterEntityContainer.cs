using Components.Common;
using Doom.GamePlay.Components.Common;
using Doom.Infrastructure.Containers.LogicContainers;
using Doom.Infrastructure.Containers.UnityApi;
using Infrastructure.Containers.UnityApi;

namespace Infrastructure.Containers.EntityContainers
{
    public abstract class UpdaterEntityContainer<TEntity> : BaseEntityContainer<TEntity>
    {
        private readonly IFrameUpdaterSubscribe _frameUpdaterSubscribe;
        private readonly IFrameFixedUpdaterSubscribe _frameFixedUpdaterSubscribe;

        protected UpdaterEntityContainer(
            IFrameUpdaterSubscribe frameUpdaterSubscribe, 
            IFrameFixedUpdaterSubscribe frameFixedUpdaterSubscribe)
        {
            _frameUpdaterSubscribe = frameUpdaterSubscribe;
            _frameFixedUpdaterSubscribe = frameFixedUpdaterSubscribe;
        }
        
        protected override void OnAdd<TAddEntity>(TAddEntity addEntity)
        {
            base.OnAdd(addEntity);
            if (addEntity is IUpdater updater)
                _frameUpdaterSubscribe.AddUpdater(updater);
            if (addEntity is IFixedUpdater fixedUpdater) 
                _frameFixedUpdaterSubscribe.AddFixedUpdater(fixedUpdater);
        }
    }
}