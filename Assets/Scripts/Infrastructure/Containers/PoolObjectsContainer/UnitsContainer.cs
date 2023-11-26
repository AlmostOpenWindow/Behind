using System;
using System.Collections.Generic;
using System.Linq;
using Components.Units;
using Configs;
using Configs.Units;
using Infrastructure.Randoms;
using Services.Debug;
using UnityEngine;

namespace Infrastructure.Containers.PoolObjectsContainer
{
    public class UnitsContainer : IUnitsContainer
    {
        private readonly ILogService _logService;
        private readonly IRandomService _randomService;

        private readonly Dictionary<ConfigID<UnitConfig>, List<Unit>> _units = new();
        private readonly Dictionary<ConfigID<UnitConfig>, Stack<Unit>> _deadUnits = new();
        // Doesn't need enemies yet
        // private readonly Dictionary<ConfigID<EnemyConfig>, List<Enemy>> _enemies = new();
        // private readonly Dictionary<ConfigID<EnemyConfig>, Stack<Enemy>> _deadEnemies = new();
        //
        private ConfigID<UnitConfig>[] _unitConfigs = Array.Empty<ConfigID<UnitConfig>>();
        
        private Hero _hero;

        public UnitsContainer(ILogService logService, IRandomService randomService)
        {
            _logService = logService;
            _randomService = randomService;
        }

        public Hero Hero
        {
            get => _hero;
            set
            {
                if (_hero != null)
                    _logService.Error("Hero is not null");
                _hero = value;
            }
        }

        // public Enemy GetMinDistEnemy(Vector3 position)
        // {
        //     (float minDist, Enemy enemy) target = (float.MaxValue, null);
        //     foreach(var enemyConfig in _enemies.Keys)
        //         foreach (var enemy in _enemies[enemyConfig])
        //         {
        //             var sqrDist = Vector3.SqrMagnitude(enemy.transform.position - position);
        //             if (target.minDist > sqrDist)
        //                 target = (sqrDist, enemy);
        //         }
        //
        //     return target.enemy;
        // }

        // public List<Enemy> GetEnemyInRadius(Vector3 point, float radius)
        // {
        //     var sqrRadius = Mathf.Pow(radius, 2);
        //
        //     var list = new List<Enemy>();
        //     foreach (var enemyConfig in _enemies.Keys)
        //     {
        //         foreach (var enemy in _enemies[enemyConfig])
        //         {
        //             if (sqrRadius > Vector3.SqrMagnitude(enemy.transform.position - _hero.transform.position))
        //             {
        //                 list.Add(enemy);
        //             }
        //         }
        //     }
        //
        //     return list;
        // }

        public GameObject GetDeadInstance(ConfigID<UnitConfig> unitConfig)
        {
            return _deadUnits.ContainsKey(unitConfig)
                   && _deadUnits[unitConfig].Any()
                       ? _deadUnits[unitConfig].Pop()?.gameObject
                       : null;
        }
        
        public bool HasUnits()
            => _units.Keys.Any(key => _units[key].Count > 0);

        public void AddUnit(ConfigID<UnitConfig> unitConfig, Unit unit)
        {
            AddToLivePool(unitConfig, unit);
        }

        private void OnUnitDead(ConfigID<UnitConfig> unitConfig, Unit unit)
        {
            unit.DeadEvent -= OnUnitDead;
            RemoveFromLiveList(unitConfig, unit);
            AddToDeadPool(unitConfig, unit);
        }

        private void RemoveFromLiveList(ConfigID<UnitConfig> unitConfig, Unit unit)
        {
            _units[unitConfig].Remove(unit);
            if (_units[unitConfig].Count != 0) 
                return;
            
            _units.Remove(unitConfig);
            UpdateLiveUnitConfigs();
        }

        private void AddToLivePool(ConfigID<UnitConfig> enemyConfig, Unit unit)
        {
            if (!_units.ContainsKey(enemyConfig))
            {
                _units.Add(enemyConfig, new List<Unit>());
                UpdateLiveUnitConfigs();
            }
            _units[enemyConfig].Add(unit);
        }

        
        private void UpdateLiveUnitConfigs()
        {
            _unitConfigs = _units.Keys.ToArray();
        }

        private void AddToDeadPool(ConfigID<UnitConfig> unitConfig, Unit unit)
        {
            if (!_deadUnits.ContainsKey(unitConfig))
                _deadUnits.Add(unitConfig, new Stack<Unit>());
            _deadUnits[unitConfig].Push(unit);
        }

        // public Enemy GetRandomEnemy()
        // {
        //     if (_enemyConfigs.Length == 0)
        //         return null;
        //     
        //     var indexConfig = _randomService.NextInt(_enemyConfigs.Length);
        //     var randomEnemy = _enemyConfigs[indexConfig];
        //     var indexEnemy = _randomService.NextInt(_enemies[randomEnemy].Count);
        //     return _enemies[randomEnemy][indexEnemy];
        // }
    }
}