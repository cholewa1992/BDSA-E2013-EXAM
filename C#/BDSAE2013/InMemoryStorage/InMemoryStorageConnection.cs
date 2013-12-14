using System;
using System.Collections.Generic;
using System.Linq;
using Storage;

namespace InMemoryStorage
{
    /// <summary>
    /// In-memory implementation of IStorageConnetion for storing entities in memory
    /// </summary>
    internal class InMemoryStorageConnection : IStorageConnection
    {
        private readonly Dictionary<Type, ISaveable> _sets = new Dictionary<Type, ISaveable>();
        private bool _isDisposed;


        /// <summary>
        /// Gets the set need for the entity currently used
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>An active in memory storage set</returns>
        private InMemoryStorageSet<TEntity> GetSet<TEntity>() where TEntity : class, IEntityDto
        {
            if (!_sets.ContainsKey(typeof (TEntity)))
            {
                _sets[typeof (TEntity)] = new InMemoryStorageSet<TEntity>();
            } 
            return (InMemoryStorageSet<TEntity>) _sets[typeof(TEntity)];
        }

        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        /// <remarks>
        /// @pre !IsDisposed
        /// </remarks>
        public IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto
        {
            IsDisposed();
            return GetSet<TEntity>().Get();
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <remarks>
        /// @pre entity.Id == 0
        /// @pre !IsDisposed
        /// </remarks>
        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            GetSet<TEntity>().Add(entity);
        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <remarks>
        /// @pre !IsDisposed
        /// </remarks>
        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            GetSet<TEntity>().Update(entity);
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        /// <remarks>
        /// @pre !IsDisposed
        /// </remarks>
        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            GetSet<TEntity>().Delete(entity);
        }

        /// <summary>
        /// Saves changes to the context
        /// </summary>
        /// <returns>true if entities was saved</returns>
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
        public bool SaveChanges()
        {
            IsDisposed();
            foreach (var kvp in _sets)
            {
                kvp.Value.SaveChanges();
            }
            return true;
        }


        /// <summary>
        /// Disposes the current context
        /// </summary>
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
        public void Dispose()
        {
            IsDisposed();
            _isDisposed = true;
        }

        private void IsDisposed()
        {
            if (_isDisposed) throw new InternalDbException("The context has been disposed");
        }
    }
}
