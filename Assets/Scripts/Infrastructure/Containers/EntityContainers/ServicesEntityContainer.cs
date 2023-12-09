using Infrastructure.Containers.UnityApi;
using Services;

namespace Infrastructure.Containers.EntityContainers
{
    public class ServicesEntityContainer : UpdaterEntityContainer<IService>
    {
        protected override string ContainerName
            => "ServiceContainer";

        protected override string EntityName
            => "service";

        public ServicesEntityContainer(
            IFrameUpdaterSubscribe frameUpdaterSubscribe, 
            IFrameFixedUpdaterSubscribe frameFixedUpdaterSubscribe,
            IFrameLateUpdaterSubscribe frameLateUpdaterSubscribe) 
            : base(frameUpdaterSubscribe, frameFixedUpdaterSubscribe, frameLateUpdaterSubscribe)
        {
        }
    }
}