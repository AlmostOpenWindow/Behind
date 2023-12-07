using System;
using Infrastructure.Containers.EntityContainers;
using Infrastructure.Containers.PoolObjectsContainer;
using Infrastructure.Factories.LevelInitializers;
using Infrastructure.Factories.Units;
using Infrastructure.GameEntryPoint;
using Infrastructure.Randoms;
using Infrastructure.Services.Windows;
using Infrastructure.Time;
using Services.Debug;

namespace Infrastructure.Tasks
{
    public class InitializedApplicationTask : ITask
    {
        public event Action FinishedTask;
        private readonly ApplicationContainer _applicationContainer;
        
        public InitializedApplicationTask(
            ApplicationContainer applicationContainer)
        {
            _applicationContainer = applicationContainer;
        }

        private ServicesEntityContainer Services
            => _applicationContainer.ServicesEntity;

        private WorldEntityContainer World
            => _applicationContainer.WorldEntity;

        private FactoriesEntityContainer Factories
            => _applicationContainer.FactoriesEntity;

        public void Init()
        {
            InitializeStarterFactories();
            InitializeInputs();
            InitializeServices();
            InitializeWorldContainers();
            InitializeFactories();
            
            SpawnApplicationInfrastructurePrefabs();
            
            FinishedTask?.Invoke();
        }

        private void InitializeStarterFactories()
        {
            Factories.Add(new ApplicationStartFactory(_applicationContainer.FrameUpdater));
        }
        
        private void InitializeInputs()
        {
            var startFactory = Factories.Get<ApplicationStartFactory>();
            var inputService = startFactory.SpawnPlayerInput();
            Services.Add(inputService);
        }

        private void InitializeServices()
        {
            Services.Add<ILogService>(new UnityLogService());
            Services.Add<ITimeService>(new UnityTimeService());
            Services.Add<IWindowService>(new WindowService(_applicationContainer));
            Services.Add<IRandomService>(new UnityRandomService(Services.Get<ILogService>()));
        }

        private void InitializeFactories()
        {
            Factories.Add<IUnitFactory>(new UnitFactory(
                _applicationContainer,
                Services.Get<ILogService>(),
                _applicationContainer.WorldEntity.Get<IUnitsContainer>(),
                _applicationContainer.FrameUpdater));
        }

        private void InitializeWorldContainers()
        {
            var unitsContainer = World.Add<IUnitsContainer>(
                new UnitsContainer(
                    Services.Get<ILogService>(),
                    Services.Get<IRandomService>()));
            World.SetUnitsContainer(unitsContainer);
        }

        private void SpawnApplicationInfrastructurePrefabs()
        {
            Factories.Get<ApplicationStartFactory>().SpawnInfrastructurePrefabs();
        }
        
        public void Update()
        {
            
        }

        public void Destroy()
        {
            
        }

        public void Run()
        {
        }
    }
}