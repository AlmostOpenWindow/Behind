using System;
using GameEntryPoint;
using Infrastructure.Containers.EntityContainers;
using Infrastructure.Containers.PoolObjectsContainer;
using Infrastructure.Randoms;
using Infrastructure.Services.Windows;
using Infrastructure.Time;
using Services.Debug;
using UnityEngine;

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
            InitializeInputs();
            InitializeServices();
            InitializeWorldContainers();
            InitializeFactories();
            
            FinishedTask?.Invoke();
        }

        private void InitializeInputs()
        {
            //todo input services
            // if (Application.isMobilePlatform)
            //     Services.Add<IInputService>(new GameplayInputService());
            // else
            //     Services.Add<IInputService>(new UnityEditorInputService());
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
           
        }

        private void InitializeWorldContainers()
        {
            var unitsContainer = World.Add<IUnitsContainer>(
                new UnitsContainer(
                    Services.Get<ILogService>(),
                    Services.Get<IRandomService>()));
            World.SetUnitsContainer(unitsContainer);
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