using System;
using System.Linq;
namespace Storage
{
    /// <summary>
    /// Bridge implementation to provide stubs to builde storage module on
    /// </summary>
    public interface IStorageConnectionBridgeFacade : IDisposable
    {
        /// <summary>
        /// Fetches a single entity from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <param name="id">The id of the entity you wish to fetch</param>
        /// <returns>The entity with the given ID. Throws an EntityNotFoundException if nothing is found</returns>
        TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto;

        /// <summary>
        /// Fetches an IQueryable with all data for a given entity typr
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <returns>An IQueryable with all the data</returns>
        IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto;

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <returns>The entity just added</returns>
        void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        void Delete<TEntity>(int id) where TEntity : class, IEntityDto;

        /// <summary>
        /// Checks whether or not the storage connection is active
        /// </summary>
        void IsDisposed();
    }
}