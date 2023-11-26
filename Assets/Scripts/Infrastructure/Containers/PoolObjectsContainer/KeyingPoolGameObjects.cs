using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Containers.PoolObjectsContainer
{
    public class KeyingPoolGameObjects<TKey, TValue>
        where TValue : MonoBehaviour, IPooledObject<TValue>
    {
        private Dictionary<TKey, PoolGameObject<TValue>> _pools = new();
        
        public bool AddToPool(TKey key, TValue value)
        {
            if (!_pools.TryGetValue(key, out var pool))
            {
                pool = new PoolGameObject<TValue>();
                _pools.Add(key, pool);
            }

            return pool.AddToPool(value);
        }

        public bool TryGetInstant(TKey key, out TValue instant)
        {
            if (!_pools.TryGetValue(key, out var pool))
            {
                pool = new PoolGameObject<TValue>();
                _pools.Add(key, pool);
            }

            instant = pool.GetInstant();
            return instant != null;
        }

        public void Remove(TKey key, TValue value)
        {
            if (!_pools.TryGetValue(key, out var pool))
            {
                return;
            }
            pool.Remove(value);
        }
    }
}