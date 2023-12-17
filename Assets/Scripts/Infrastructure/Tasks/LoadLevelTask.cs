using System;
using Components.Common;
using Infrastructure.Factories.Units;
using Infrastructure.GameEntryPoint;
using Infrastructure.Mechanics;
using Infrastructure.Mechanics.Interact;
using Infrastructure.Mechanics.PerformShip;
using Infrastructure.Mechanics.Routines;
using Infrastructure.Services.Input;

namespace Infrastructure.Tasks
{
    public class LoadLevelTask : ITask
    {
        public event Action FinishedTask;
        private readonly ApplicationContainer _applicationContainer;
        private readonly ConfigCatalogs _configCatalogs;
        
        public LoadLevelTask(ApplicationContainer applicationContainer)
        {
            _applicationContainer = applicationContainer;
            _configCatalogs = _applicationContainer.ConfigCatalogs;

        }

        public void Init()
        {
            switch (_applicationContainer.SceneData.StartSceneType)
            {
                case StartSceneType.StartFromPlayer:
                    _applicationContainer.SceneData.HeroFollowCamera.gameObject.SetActive(true);
                    _applicationContainer.SceneData.ShipFollowCamera.gameObject.SetActive(false);
                    SpawnHero();
                    break;
                case StartSceneType.StartFromShip:
                    _applicationContainer.SceneData.HeroFollowCamera.gameObject.SetActive(false);
                    _applicationContainer.SceneData.ShipFollowCamera.gameObject.SetActive(true);
                    SpawnShip(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            InitializeMechanics();
            FinishedTask?.Invoke();
        }

        private void InitializeMechanics()
        {
            var interactMechanicsConfig =
                _applicationContainer.ConfigCatalogs.MechanicsConfigsCatalog.InteractMechanicsConfig;
            var interactMechanics = new InteractMechanics(interactMechanicsConfig);
            if (interactMechanicsConfig.EnableMechanics)
                interactMechanics.Enable();
            
            _applicationContainer.MechanicContainer.Add<IInteractMechanics>(interactMechanics);

            var mountMechanicsConfig = _applicationContainer.ConfigCatalogs.MechanicsConfigsCatalog.MountMechanicsConfig;
            var shipMountMechanics = new MountMechanics(
                mountMechanicsConfig,
                interactMechanics,
                _applicationContainer.ServicesEntity.Get<IInputService>(),
                _applicationContainer.WorldEntity.Units,
                _applicationContainer.SceneData);
            if (mountMechanicsConfig.EnableMechanics)
                shipMountMechanics.Enable();
            else
                shipMountMechanics.Disable();
            
            _applicationContainer.MechanicContainer.Add<IMountMechanics>(shipMountMechanics);

            var routineMechanicsConfig = _configCatalogs.MechanicsConfigsCatalog.RoutineMechanicsConfig;
            var routineMechanics = new RoutineMechanics();
            if (routineMechanicsConfig.EnableMechanics)
                routineMechanics.Enable();
            else
                routineMechanics.Disable();
            
            _applicationContainer.MechanicContainer.Add<IRoutineMechanics>(routineMechanics);

            var performShipMechanicsConfig = _configCatalogs.MechanicsConfigsCatalog.PerformShipMechanicsConfig;
            var performShipMechanics = new PerformShipMechanics(
                performShipMechanicsConfig,
                interactMechanics,
                routineMechanics,
                _applicationContainer.FactoriesEntity.Get<IUnitFactory>(),
                _applicationContainer.ServicesEntity.Get<IInputService>(),
                _applicationContainer.SceneData);
            if (performShipMechanicsConfig.EnableMechanics)
                performShipMechanics.Enable();
            else
                performShipMechanics.Disable();
            
            _applicationContainer.MechanicContainer.Add<IPerformShipMechanics>(performShipMechanics);
        }

        private void SpawnHero()
        {
            var unitFactory = _applicationContainer.FactoriesEntity.Get<IUnitFactory>();
            unitFactory.SpawnHero();
        }

        private void SpawnShip(bool activated)
        {
            var unitFactory = _applicationContainer.FactoriesEntity.Get<IUnitFactory>();
            unitFactory.SpawnShip(activated);
        }
        
        public void Update()
        {
            
        }

        public void Destroy()
        {
            
        }
    }
}