using System;
using System.Data;
using System.Data.Entity.Validation;
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
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
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
        /// <remarks>
        /// @pre entity.Id != 0
        /// @pre IsDisposed == false
        /// </remarks>
        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            if (entity.Id == 0) throw new InternalDbException("The Id of an new entity should be set!");
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
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
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
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            IsDisposed();
            _ef.Entry(entity).State = EntityState.Deleted;
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
            try
            {
                return _ef.SaveChanges() > 0;
            }
            catch (DbEntityValidationException)
            {
                Dispose();
                throw new InternalDbException("The entities you tried to save violated a db contraint");
            }
            catch (EntityException)
            {
                throw new InternalDbException("The connection to the database failed or timed out");
            }
            catch(Exception e)
            {
                Dispose();
                throw new InternalDbException("The data was not saved due to an unexpected error", e);
            }
        }

        private void IsDisposed()
        {
            if(_isDisposed) throw new InternalDbException("The context has been disposed");
        }

        /// <summary>
        /// Disposes the current context
        /// </summary>
        /// <remarks>
        /// @pre IsDisposed == false
        /// </remarks>
        public void Dispose()
        {
            _isDisposed = true;
            _ef.Dispose();
        }
    }
}
