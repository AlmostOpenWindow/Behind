using System;
using Doom.Infrastructure.Containers.LogicContainers;
using Infrastructure.Containers.PoolObjectsContainer;

namespace Infrastructure.Containers.EntityContainers
{
    public class WorldEntityContainer : BaseEntityContainer<IPoolObjectsContainer>
    {
        protected override string ContainerName
            => "World";

        protected override string EntityName
            => "pool";
        
        public IUnitsContainer Units { get; private set; }

        public void SetUnitsContainer(IUnitsContainer units)
        {
            if (Units != null)
                throw new Exception("Unity API Container already initialized");

            Units = units;
        }
    }
}