namespace Infrastructure.Factories.Units
{
    public interface IUnitFactory : IFactory
    {
        void SpawnShip(bool activated);
        void SpawnHero();
    }
}