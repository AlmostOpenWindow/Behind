using Components.Common;
using Components.Units;
using Configs.Units;
using Infrastructure.Containers.PoolObjectsContainer;
using Infrastructure.Containers.UnityApi;
using Infrastructure.GameEntryPoint;
using Infrastructure.Services.Input;
using Services.Debug;
using UnityEngine;

namespace Infrastructure.Factories.Units
{
    public class UnitFactory : BaseFactory, IUnitFactory
    {
        private readonly ApplicationContainer _applicationContainer;
        private readonly ILogService _logService;
        private readonly IUnitsContainer _unitsContainer;
        private readonly IFrameMonoControllerSubscribe _frameMonoControllerSubscribe;
        
        public UnitFactory(
            ApplicationContainer applicationContainer, 
            ILogService logService, 
            IUnitsContainer unitsContainer,
            IFrameMonoControllerSubscribe frameMonoControllerSubscribe)
        {
            _applicationContainer = applicationContainer;
            _logService = logService;
            _unitsContainer = unitsContainer;
            _frameMonoControllerSubscribe = frameMonoControllerSubscribe;
        }

        public void SpawnShip()
        {
            var sceneData = _applicationContainer.SceneData;
            var shipConfig = _applicationContainer.ConfigCatalogs.UnitConfigsCatalog.ShipConfig;
            var shipObj = Object.Instantiate(
                Load(shipConfig.PrefabPath),
                sceneData.ShipInitialPoint.position,
                sceneData.ShipInitialPoint.rotation);

            var shipComponent = shipObj.GetComponent<Ship>();
            sceneData.ShipFollowCamera.Follow = shipComponent.CameraRoot;
            shipComponent.Construct(
                shipConfig.ToConfigID<ShipConfig>(), 
                _applicationContainer.ServicesEntity.Get<IInputService>(), 
                sceneData.GameplayCamera);
            
            SubscribeMonoController(shipComponent);
        }

        public void SpawnHero()
        {
            var heroConfig = _applicationContainer.ConfigCatalogs.UnitConfigsCatalog.HeroConfig;
        }

        public void SpawnUnit()
        {
            
        }
        
        private void SubscribeMonoController(MonoController monoController)
        {
            _frameMonoControllerSubscribe?.AddMonoController(monoController);
        }
    }
}