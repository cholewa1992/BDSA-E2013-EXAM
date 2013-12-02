using System;
using System.Linq;

namespace Storage
{
    /// <summary>
    /// Refined IStorageBridge implementation.
    /// </summary>
    public class StorageBridgeFacade : StorageConnectionBridgeFacade
    {
        /// <summary>
        /// Constructs a new StorageBridgeFacade
        /// </summary>
        /// <param name="storageFactory">The storage to use</param>
        /// <remarks>
        /// </remarks>
        public StorageBridgeFacade(IStorageConnectionFactory storageFactory)
            : base(storageFactory)
        {
        }

        /// <summary>
        /// Fetches a single entity from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <param name="id">The id of the entity you wish to fetch</param>
        /// <returns>The entity with the given ID. Throws an EntityNotFoundException if nothing is found</returns>
        /// <remarks>
        /// @pre id > 0
        /// @pre id < int.max
        /// </remarks>
        public override TEntity Get<TEntity>(int id)
        {
            IsDisposed();
            if(id <= 0){ throw new ArgumentException("Ids 0 or less"); }
            if (id > int.MaxValue) { throw new ArgumentException("Ids larger than int.MaxValue"); }
            try
            {
                return Db.Get<TEntity>().Single(t => t.Id == id);
            }
            catch(InvalidOperationException)
            {
                throw new InvalidOperationException("Either none or too many entities with given ID was found");
            }
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
        /// <remarks>
        /// @pre entity != null
        /// @pre entity.Id == 0
        /// @pre IsDisposed();
        /// @post entity.Id != 0
        /// </remarks>
        public override bool Add<TEntity>(TEntity entity)
        {
            IsDisposed();
            if(entity == null) throw new ArgumentNullException("entity");
            if(entity.Id != 0) throw new ArgumentException("Id can't be preset!");

            Db.Add(entity);

            if(entity.Id == 0) throw new InternalDbException("Id was not set");

            return SaveChanges();
        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <returns>The just updated entity</returns>
        /// <remarks>
        /// @pre IsDisposed();
        /// @pre entity != null
        /// </remarks>
        public override bool Update<TEntity>(TEntity entity)
        {
            IsDisposed();
            if (entity == null) throw new ArgumentNullException("entity");
            Db.Update(entity);
            return SaveChanges();
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        /// <remarks>
        /// @pre IsDisposed();
        /// @pre entity != null
        /// </remarks>
        public override bool Delete<TEntity>(TEntity entity)
        {
            IsDisposed();
            if (entity == null) throw new ArgumentNullException("entity");
            Db.Delete(entity);
            return SaveChanges();
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns>True if the operation was successfull</returns>
        /// <remarks>
        /// @pre IsDisposed();
        /// @pre id >= 0
        /// @pre id < int.max
        /// </remarks>
        public override bool Delete<TEntity>(int id)
        {
            IsDisposed();
            if (id <= 0) { throw new ArgumentException("Ids 0 or less"); }
            if (id > int.MaxValue) { throw new ArgumentException("Ids larger than int.MaxValue"); }
            Db.Delete(Get<TEntity>(id));
            return SaveChanges();
        }

    }
}
