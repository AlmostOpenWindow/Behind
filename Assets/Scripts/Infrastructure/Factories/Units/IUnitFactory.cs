namespace Infrastructure.Factories.Units
{
    public interface IUnitFactory : IFactory
    {
        void SpawnShip();
        void SpawnHero();
    }
}