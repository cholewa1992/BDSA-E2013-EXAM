using System;
using System.Data;
using System.Linq;
using Storage;

namespace EntityFrameworkStorage
{
    /// <summary>
    /// Represents an Entity framework connection
    /// </summary>
    /// <typeparam name="TContext">The context to connect to</typeparam>
    public class EFStorageConnection<TContext> : IStorageConnection where TContext : IDbContext, new()
    {
        private readonly TContext _ef;
        private bool _isDisposed;

        /// <summary>
        /// Contructs an EFStorageConnection
        /// </summary>
        internal EFStorageConnection()
        {
            _ef = new TContext();
        }

        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        public IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto
        {
            IsDisposed();
            return _ef.Set<TEntity>();
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <returns>The entity just added</returns>
        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            if(entity.Id != 0) throw new InvalidOperationException("The Id of an new entity should not be set!");
            try
            {
                entity.Id = Get<TEntity>().Max(t => t.Id) + 1;
            }
            catch(Exception)
            {
                entity.Id = 1;
            }
            _ef.Entry(entity).State = EntityState.Added;
            _ef.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            _ef.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            _ef.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Saves changes to the context
        /// </summary>
        /// <returns>true if entities was saved</returns>
        public bool SaveChanges()
        {
            IsDisposed();
            return _ef.SaveChanges() > 0;
        }

        private void IsDisposed()
        {
            if(_isDisposed) throw new InvalidOperationException("The context has been disposed");
        }

        /// <summary>
        /// Disposes the current context
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
            _ef.Dispose();
        }
    }
}
