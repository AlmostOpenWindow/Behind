using System;
using Components.Common;
using Infrastructure.Containers.PoolObjectsContainer;

namespace Doom.GamePlay.Components.Common
{
    //todo: подумать о том, чтобы все MonoController сделать PooledMonoController
    public abstract class PooledMonoController<TStrategy> 
        : MonoController, IPooledObject<TStrategy>  
        where TStrategy : PooledMonoController<TStrategy>
    {
        public event Action<TStrategy, bool> PooledObjectChangeStateEvent;
        protected abstract TStrategy PooledStrategy { get; }

        protected override void OnEnableSet()
        {
            base.OnEnableSet();
            PooledObjectChangeStateEvent?.Invoke(PooledStrategy, true);
        }

        protected override void OnDisableSet()
        {
            base.OnDisableSet();
            PooledObjectChangeStateEvent?.Invoke(PooledStrategy, false);
        }
    }
}