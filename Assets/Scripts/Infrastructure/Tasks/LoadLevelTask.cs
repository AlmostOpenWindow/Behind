using System;
using Infrastructure.Factories.Units;
using Infrastructure.GameEntryPoint;

namespace Infrastructure.Tasks
{
    public class LoadLevelTask : ITask
    {
        public event Action FinishedTask;
        private readonly ApplicationContainer _applicationContainer;

        public LoadLevelTask(ApplicationContainer applicationContainer)
        {
            _applicationContainer = applicationContainer;
        }

        public void Init()
        {
            //don't forget enable mechanics after init _applicationContainer.MechanicContainer.Enable();
            SpawnShip();
            FinishedTask?.Invoke();
        }

        private void SpawnHero()
        {
            //todo SpawnHero function
        }

        private void SpawnShip()
        {
            var unitFactory = _applicationContainer.FactoriesEntity.Get<IUnitFactory>();
            unitFactory.SpawnShip();
        }
        
        public void Update()
        {
            
        }

        public void Destroy()
        {
            
        }
    }
}