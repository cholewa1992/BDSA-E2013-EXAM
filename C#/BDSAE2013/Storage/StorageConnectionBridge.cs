using System.Linq;

namespace Storage
{
    /// <summary>
    /// Bridge implementation to provide stubs to builde storage module around
    /// </summary>
    public abstract class StorageConnectionBridge : IStorageConnectionBridge
    {
        /// <summary>
        /// Concret IStorageFactory implementation to use
        /// </summary>
        protected IStorageConnection Db { private set; get; }

        /// <summary>
        /// Constructs the bridge and uses dependency injection of an conret storage to use
        /// </summary>
        /// <param name="storageFactory">Concret storage implementation to use</param>
        protected StorageConnectionBridge(IStorageConnectionFactory storageFactory)
        {
            Db = storageFactory.GetConnection();
        }

        /// <summary>
        /// Disposable methode to ensure that the bridge and its underlying storage is closed corretly 
        /// </summary>
        public void Dispose()
        {
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
        /// <returns>The entity just added</returns>
        public abstract bool Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        public abstract bool Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public abstract bool Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto;

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public abstract bool Delete<TEntity>(int id) where TEntity : class, IEntityDto;

        /// <summary>
        /// Saves changes to the context
        /// </summary>
        /// <returns>true if entities was saved</returns>
        protected bool SaveChanges()
        {
            return Db.SaveChanges();
        }
    }
}
