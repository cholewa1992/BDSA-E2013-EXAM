using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Storage;

namespace FlatFileStorage
{
    /// <summary>
    /// In memory storage set implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity for saving entities</typeparam>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class InMemoryStorage<TEntity> : ISaveable, IDisposable where TEntity : class, IEntityDto
    {
        public static SynchronizedCollection<TEntity> Entities = new SynchronizedCollection<TEntity>(new EntityCompare());
        private HashSet<EntityEntryDto> _states;

        public InMemoryStorage()
        {
            _states = new HashSet<EntityEntryDto>();
        }

        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        public IQueryable<TEntity> Get()
        {
            lock (Entities)
            return Entities.ToList().AsQueryable();
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <returns>The entity just added</returns>
        /// <remarks>
        /// @pre entity.Id == 0
        /// </remarks>
        public void Add(TEntity entity)
        {
            if (entity.Id != 0) throw new InternalDbException("Id can not be preset!");
            _states.Add(new EntityEntryDto
            {
                Id = entity.Id,
                EntityType = entity.GetType(),
                State = EntityState.Added,
                Entity = entity
            });
        }


        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        /// <remarks>
        /// @pre entity.Id != 0
        /// </remarks>
        public void Update(TEntity entity)
        {
            if (entity.Id == 0) throw new InternalDbException("Id must be set!");
            _states.Add(new EntityEntryDto
            {
                Id = entity.Id,
                EntityType = entity.GetType(),
                State = EntityState.Modified,
                Entity = entity
            });
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public void Delete(TEntity entity)
        {
            if (entity.Id == 0) throw new InternalDbException("Id must be set!");
            _states.Add(new EntityEntryDto
            {
                Id = entity.Id,
                EntityType = entity.GetType(),
                State = EntityState.Deleted,
                Entity = entity
            });
        }

        /// <summary>
        /// Saves changes to the context
        /// </summary>
        /// <returns>true if entities was saved</returns>
        public void SaveChanges()
        {
            lock (Entities)
            {
                foreach (var o in _states)
                {
                    if (o.State == EntityState.Added)
                    {
                        try
                        {
                            o.Entity.Id = Get().Max(t => t.Id) + 1;
                        }
                        catch
                        {
                            o.Entity.Id = 1;
                        }
                        Entities.Add(o.Entity);
                    }
                    else if (o.State == EntityState.Modified)
                    {
                        Entities.Remove(Entities.Single(t => t.Id == o.Id));
                        Entities.Add(o.Entity);
                    }
                    else if (o.State == EntityState.Deleted)
                    {
                        Entities.Remove(Entities.Single(t => t.Id == o.Id));

                    }
                }
                _states = new HashSet<EntityEntryDto>();
            }
        }

        /// <summary>
        /// Disposes the current context
        /// </summary>
        public void Dispose()
        {

        }

        private class EntityEntryDto
        {
            public int Id { set; get; }
            public Type EntityType { set; get; }
            public EntityState State { set; get; }
            public TEntity Entity { set; get; }
        }

        private class EntityCompare : IEqualityComparer<TEntity>
        {

            public bool Equals(TEntity b1, TEntity b2)
            {
                return b1.Id == b2.Id;
            }

            public int GetHashCode(TEntity bx)
            {
                return (bx.Id + bx.GetType().GetHashCode()).GetHashCode();
            }

        }
    }
}
