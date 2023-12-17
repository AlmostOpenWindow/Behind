using Components.Common;
using Components.Units;
using Configs.Units;
using GameEvents.Configs;
using GameEvents.Data;
using GameEvents.Listeners;
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
        private readonly GameEventListener _gameEventListener;
        
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

        public void SpawnShip(bool activated)
        {
            var sceneData = _applicationContainer.SceneData;
            sceneData.ShipFollowCamera.gameObject.SetActive(activated);
            
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

            _unitsContainer.Ship = shipComponent;
            SubscribeMonoController(shipComponent);
            
            if (activated)
                shipComponent.Resume();
            else
                shipComponent.Pause();
        }

        public void SpawnHero()
        {
            var sceneData = _applicationContainer.SceneData;
            sceneData.HeroFollowCamera.gameObject.SetActive(true);
            
            var heroConfig = _applicationContainer.ConfigCatalogs.UnitConfigsCatalog.HeroConfig;
            var heroObj = Object.Instantiate(
                Load(heroConfig.PrefabPath),
                sceneData.HeroInitialPoint.position,
                sceneData.HeroInitialPoint.rotation);

            var heroComponent = heroObj.GetComponent<Hero>();
            sceneData.HeroFollowCamera.Follow = heroComponent.CameraRoot;
            heroComponent.Construct(
                heroConfig.ToConfigID<HeroConfig>(),
                _applicationContainer.ServicesEntity.Get<IInputService>(),
                sceneData.GameplayCamera);

            _unitsContainer.Hero = heroComponent;
            SubscribeMonoController(heroComponent);
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