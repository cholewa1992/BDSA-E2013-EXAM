using System.Linq;

namespace Storage
{
    public class RefinedStorageConnectionBridge : StorageConnectionBridge
    {
        public RefinedStorageConnectionBridge(IStorageConnectionFactory storageFactory) : base(storageFactory)
        {
        }

        /// <summary>
        /// Fetches a single entity from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <param name="id">The id of the entity you wish to fetch</param>
        /// <returns>The entity with the given ID. Throws an EntityNotFoundException if nothing is found</returns>
        public override TEntity Get<TEntity>(int id)
        {
            return Db.Get<TEntity>(id);

        }

        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        public override IQueryable<TEntity> Get<TEntity>()
        {
            return Db.Get<TEntity>();
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <returns>The entity just added</returns>
        public override void Add<TEntity>(TEntity entity)
        {
            Db.Add(entity);
        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        public override void Update<TEntity>(TEntity entity)
        {
            Db.Update(entity);
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public override void Delete<TEntity>(TEntity entity)
        {
            Db.Delete(entity);
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public override void Delete<TEntity>(int id)
        {
            Db.Delete(Get<TEntity>(id));
        }

    }
}
