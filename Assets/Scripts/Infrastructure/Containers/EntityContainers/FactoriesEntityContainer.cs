using Infrastructure.Factories;

namespace Infrastructure.Containers.EntityContainers
{
    public class FactoriesEntityContainer : BaseEntityContainer<IFactory>
    {

        protected override string ContainerName
            => "Factories";

        protected override string EntityName
            => "factory";
    }
}