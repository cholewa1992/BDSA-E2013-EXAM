using System.Collections.Generic;


namespace Storage
{
    internal class RefinedStorageBidge : StorageBridge
    {
        public RefinedStorageBidge(IStorageFactory storageFactory) : base(storageFactory)
        {
        }

        /// <summary>
        /// Disposable methode to ensure that the bridge and its underlying storage is closed corretly 
        /// </summary>
        public override void Dispose()
        {

        }

        /// <summary>
        /// Method for getting entities
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The query result</returns>
        public override IEnumerable<TEntity> Get<TEntity>()
        {
            using (var db = StorageFactory.GetConnection())
            {
                return db.Get<TEntity>();
            }
        }

        /// <summary>
        /// Fetches a single entity from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <param name="id">The id of the entity you wish to fetch</param>
        /// <returns>The entity with the given ID. Throws an EntityNotFoundException if nothing is found</returns>
        public override TEntity Get<TEntity>(int id)
        {
            using (var db = StorageFactory.GetConnection())
            {
                return db.Get<TEntity>(id);
            }
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <returns>The entity just added</returns>
        public override TEntity Add<TEntity>(TEntity entity)
        {
            using (var db = StorageFactory.GetConnection())
            {
                return db.Add(entity);
            }
        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        public override TEntity Update<TEntity>(TEntity entity)
        {
            using (var db = StorageFactory.GetConnection())
            {
                return db.Update(entity);
            }
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        public override bool Delete<TEntity>(TEntity entity)
        {
            using (var db = StorageFactory.GetConnection())
            {
                return db.Delete(entity);
            }
        }
    }
}
