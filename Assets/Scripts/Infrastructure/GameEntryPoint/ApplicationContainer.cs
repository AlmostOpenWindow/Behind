using Components.Common;
using Configs.Units;
using Infrastructure.Containers.EntityContainers;
using Infrastructure.Containers.UnityApi;
using UnityEngine;

namespace Infrastructure.GameEntryPoint
{
    public class ApplicationContainer
    {
        public readonly FrameUpdater FrameUpdater;
        public readonly SceneData SceneData;
        public readonly ServicesEntityContainer ServicesEntity;
        public readonly FactoriesEntityContainer FactoriesEntity;
        public readonly MechanicContainer MechanicContainer;
        public readonly WorldEntityContainer WorldEntity;
        public readonly ConfigCatalogs ConfigCatalogs;
        
        public ApplicationContainer(SceneData sceneData)
        {
            ConfigCatalogs = new ConfigCatalogs();
            FrameUpdater = new FrameUpdater();
            SceneData = sceneData;
            ServicesEntity = new ServicesEntityContainer(FrameUpdater, FrameUpdater, FrameUpdater);
            FactoriesEntity = new FactoriesEntityContainer();
            MechanicContainer = new MechanicContainer(FrameUpdater, FrameUpdater, FrameUpdater);
            WorldEntity = new WorldEntityContainer();
        }
    }

    public class ConfigCatalogs
    {
        private static string UnitConfigsCatalogPath => "Configs/Units/UnitConfigsCatalog";
        public readonly UnitConfigsCatalog UnitConfigsCatalog = Resources.Load<UnitConfigsCatalog>(UnitConfigsCatalogPath);
    }
}