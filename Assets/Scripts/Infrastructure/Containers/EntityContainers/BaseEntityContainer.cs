using System;
using System.Collections.Generic;

namespace Infrastructure.Containers.EntityContainers
{
    public abstract class BaseEntityContainer<TEntity>
    {
        protected readonly Dictionary<Type, TEntity> _entities;
        
        protected BaseEntityContainer()
        {
            _entities = new Dictionary<Type, TEntity>();
        }

        protected abstract string ContainerName { get; }
        protected abstract string EntityName { get; }
        
        private string EntityNotInitialised
            => "{0} not initialised {1}";

        private string AddEntityIsNull
            => "{0} is null";

        private string TryGetNullEntityError
            => "Not unique {0} {1}";

        public TGetEntity Get<TGetEntity>() where TGetEntity : TEntity
        {
            var type = typeof(TGetEntity);
            if (IsHasEntity<TGetEntity>()) 
                return (TGetEntity)_entities[type];
            var errorMessage = string.Format(EntityNotInitialised, ContainerName, typeof(TGetEntity));
            throw new ArgumentException(errorMessage);
        }

        public TAddEntity Add<TAddEntity>(TAddEntity entity) where TAddEntity : TEntity
        {
            if (entity == null)
            {
                var errorMessage = string.Format(AddEntityIsNull, ContainerName);
                throw new ArgumentException(errorMessage);
            }
            
            if (IsHasEntity<TAddEntity>())
            {
                var errorMessage = string.Format(TryGetNullEntityError, EntityName, typeof(TAddEntity));
                throw new ArgumentException(errorMessage);
            }

            _entities.Add(typeof(TAddEntity), entity);
            OnAdd(entity);
            
            return entity;
        }

        protected virtual void OnAdd<TAddEntity>(TAddEntity entity)
        {
            
        }

        private bool IsHasEntity<THasEntity>() where THasEntity: TEntity
        {
            return _entities.ContainsKey(typeof(THasEntity));
        }
    }
}