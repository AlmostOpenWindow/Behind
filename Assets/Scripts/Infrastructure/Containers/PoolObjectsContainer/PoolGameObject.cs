using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Containers.PoolObjectsContainer
{
    public class PoolGameObject<T> 
        where T : MonoBehaviour, IPooledObject<T>
    {
        protected readonly List<T> _pool = new();
        protected readonly List<T> _disabledPool = new();
        
        public bool AddToPool(T component)
            => component != null && AddComponent(component);

        public T GetInstant()
        {
            return _disabledPool.Count > 0 ? _disabledPool[0] : null;
        }

        public void Remove(T pooledObject)
        {
            pooledObject.PooledObjectChangeStateEvent -= OnComponentEnabledChange;
            _pool.Remove(pooledObject);
            if (_disabledPool.Contains(pooledObject))
                _disabledPool.Remove(pooledObject);
        }

        private bool AddComponent(T component)
        {
            if (_pool.Contains(component))
                return false;
            
            _pool.Add(component);
            if (!component.gameObject.activeSelf)
                _disabledPool.Add(component);
            
            component.PooledObjectChangeStateEvent += OnComponentEnabledChange;
            return true;
        }

        private void OnComponentEnabledChange(T pooledObject, bool state)
        {
            if (state)
                _disabledPool.Remove(pooledObject);
            else
                _disabledPool.Add(pooledObject);
        }
    }
}