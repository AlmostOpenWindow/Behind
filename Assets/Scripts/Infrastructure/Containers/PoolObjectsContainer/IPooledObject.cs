using System;
using UnityEngine;

namespace Infrastructure.Containers.PoolObjectsContainer
{
    public interface IPooledObject<out T> where T: MonoBehaviour
    {
        event Action<T, bool> PooledObjectChangeStateEvent;
    }
}