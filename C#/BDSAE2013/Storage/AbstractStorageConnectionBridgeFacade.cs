using System.Linq;

namespace Storage
{
    /// <summary>
    /// Bridge implementation to provide stubs to builde storage module on
    /// </summary>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public abstract class AbstractStorageConnectionBridgeFacade : IStorageConnectionBridgeFacade
    {
        /// <summary>
        /// Concret IStorageFactory implementation to use
        /// </summary>
        protected IStorageConnection Db { private set; get; }
        private bool _isDisposed;

        /// <summary>
        /// Constructs the bridge and uses dependency injection of an conret storage to use
        /// </summary>
        /// <param name="storageFactory">Concret storage implementation to use</param>
        protected AbstractStorageConnectionBridgeFacade(IStorageConnectionFactory storageFactory)
        {
            Db = storageFactory.CreateConnection();
        }

        /// <summary>
        /// Checks wether or not the storage connection is active
        /// </summary>
        public void IsDisposed()
        {
            if (_isDisposed) throw new InternalDbException("Storage has been disposed");
        }

        /// <summary>
        /// Disposable methode to ensure that the bridge and its underlying storage is closed corretly 
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
            Db.Dispose();
        }

        /// <summary>
        /// Fetches a single entity from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <param name="id">The id of the entity you wish to fetch</param>
        /// <returns>The entity with the given ID. Throws an EntityNotFoundException if nothing is found</returns>
        public abstract TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto;


        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        public abstract IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto;

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        public abstract void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        public abstract void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        public abstract void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        public abstract void Delete<TEntity>(int id) where TEntity : class, IEntityDto;

        /// <summary>
        /// Saves changes to the context
        /// </summary>
        protected void SaveChanges()
        {
            IsDisposed();
            if (!Db.SaveChanges())
            {
                Dispose();
                throw new ChangesWasNotSavedException("The changes in this context could not be saved!");
            }
        }
    }
}
