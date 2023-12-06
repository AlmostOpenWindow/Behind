using GameEntryPoint;
using Infrastructure.Containers.PoolObjectsContainer;
using Services.Debug;

namespace Infrastructure.Factories.Units
{
    public class UnitFactory : BaseFactory, IUnitFactory
    {
        private readonly ApplicationContainer _applicationContainer;
        private readonly ILogService _logService;
        private readonly IUnitsContainer _unitsContainer;

        public UnitFactory(ApplicationContainer applicationContainer, ILogService logService, IUnitsContainer unitsContainer)
        {
            _applicationContainer = applicationContainer;
            _logService = logService;
            _unitsContainer = unitsContainer;
        }

        public void SpawnShip()
        {
            
        }

        public void SpawnHero()
        {
            
        }

        public void SpawnUnit()
        {
            
        }
    }
}