using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Storage;

namespace InMemoryStorage
{
    /// <summary>
    /// In memory storage set implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity for saving entities</typeparam>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class InMemoryStorageSet<TEntity> : ISaveable, IDisposable where TEntity : class, IEntityDto
    {
        internal static SynchronizedCollection<TEntity> Entities = new SynchronizedCollection<TEntity>(new EntityCompare());
        private HashSet<EntityEntryDto> _states;

        /// <summary>
        /// Constructs a new In-Memory storage for storing IEntityDTO objects
        /// </summary>
        internal InMemoryStorageSet()
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
            {
                return Entities.ToList().AsQueryable();
            }
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <remarks>
        /// @pre entity.Id == 0
        /// </remarks>
        public void Add(TEntity entity)
        {
            if (entity.Id != 0) throw new InternalDbException("Id must be set!");
            _states.Add(new EntityEntryDto
            {
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
                        catch (InvalidOperationException)
                        {
                            o.Entity.Id = 1;
                        }
                        Entities.Add(o.Entity);
                    }
                    else if (o.State == EntityState.Modified)
                    {
                        try
                        {
                            Entities.Remove(Entities.Single(t => t.Id == o.Entity.Id));
                            Entities.Add(o.Entity);
                        }
                        catch
                        {
                            throw new InternalDbException("No entites with that id found");
                        }
                    }
                    else if (o.State == EntityState.Deleted)
                    {
                        try
                        {
                            Entities.Remove(Entities.Single(t => t.Id == o.Entity.Id));
                        }
                        catch(Exception e)
                        {
                            throw new InternalDbException("No entites with that id found", e);
                        }

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

        //Helper class to keep track of unsaved changes
        private class EntityEntryDto
        {
            public EntityState State { set; get; }
            public TEntity Entity { set; get; }
        }

        //Helper class for comparing entities
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

        //Helper methode for tests 
        internal static void Clear()
        {
            Entities = new SynchronizedCollection<TEntity>();
        }
    }
}
