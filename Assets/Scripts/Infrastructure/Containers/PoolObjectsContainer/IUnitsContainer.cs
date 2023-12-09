using Components.Units;
using Configs;
using Configs.Units;
using UnityEngine;

namespace Infrastructure.Containers.PoolObjectsContainer
{
    public interface IUnitsContainer : IPoolObjectsContainer
    {
        Hero Hero { get; set; }
        Ship Ship { get; set; }
        
        // Enemy GetRandomEnemy();
        // Enemy GetMinDistEnemy(Vector3 position);
        // List<Enemy> GetEnemyInRadius(Vector3 point, float radius);

        void AddUnit(ConfigID<UnitConfig> unitConfig, Unit unit);
        GameObject GetDeadInstance(ConfigID<UnitConfig> unitConfig);
        bool HasUnits();
    }
}