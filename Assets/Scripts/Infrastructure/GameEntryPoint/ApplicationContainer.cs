using Components.Common;
using Doom.GamePlay.Components.Common;
using Infrastructure.Containers.EntityContainers;
using Infrastructure.Containers.UnityApi;

namespace GameEntryPoint
{
    public class ApplicationContainer
    {
        public readonly FrameUpdater FrameUpdater;
        public readonly SceneData SceneData;
        public readonly ServicesEntityContainer ServicesEntity;
        public readonly FactoriesEntityContainer FactoriesEntity;
        public readonly MechanicContainer MechanicContainer;
        public readonly WorldEntityContainer WorldEntity;

        public ApplicationContainer(SceneData sceneData)
        {
            FrameUpdater = new FrameUpdater();
            SceneData = sceneData;
            ServicesEntity = new ServicesEntityContainer(FrameUpdater, FrameUpdater);
            FactoriesEntity = new FactoriesEntityContainer();
            MechanicContainer = new MechanicContainer(FrameUpdater, FrameUpdater);
            WorldEntity = new WorldEntityContainer();
        }
    }
}