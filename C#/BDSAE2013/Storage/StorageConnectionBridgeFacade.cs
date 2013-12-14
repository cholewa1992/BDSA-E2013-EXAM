using System;
using System.Linq;

namespace Storage
{
    /// <summary>
    /// Refined IStorageBridge implementation.
    /// </summary>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class StorageConnectionBridgeFacade : AbstractStorageConnectionBridgeFacade
    {
        /// <summary>
        /// Constructs a new StorageBridgeFacade
        /// </summary>
        /// <param name="storageFactory">The storage to use</param>
        public StorageConnectionBridgeFacade(IStorageConnectionFactory storageFactory)
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
        /// @pre int.max > id
        /// @pre !IsDisposed
        /// </remarks>
        public override TEntity Get<TEntity>(int id)
        {
            IsDisposed(); //Checks that the context is not disposed
            if (id <= 0) { throw new ArgumentException("Ids 0 or less"); } //Checks that he id is not 0 or less
            if (id > int.MaxValue) { throw new ArgumentException("Ids larger than int.MaxValue"); } //Checks the id is not more than int.MaxValue
            
            try{ return Db.Get<TEntity>().Single(t => t.Id == id); } //Trying the fetch entity with given id
            catch(InvalidOperationException) { //If the entity could not be found, throws an exception to the client
                throw new InvalidOperationException("Either none or too many entities with given ID was found");
            }
        }

        /// <summary>
        /// Fetches entities from the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to fetch</typeparam>
        /// <returns>The entities as an IQueryable</returns>
        /// <remarks>
        /// @pre !IsDisposed()
        /// </remarks>
        public override IQueryable<TEntity> Get<TEntity>()
        {
            IsDisposed();
            return Db.Get<TEntity>();
        }

        /// <summary>
        /// Adds a new entity to the storage
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add to the storage</param>
        /// <remarks>
        /// @pre entity != null
        /// @pre entity.Id == 0
        /// @pre !IsDisposed();
        /// @post entity.Id != 0
        /// </remarks>
        public override void Add<TEntity>(TEntity entity)
        {
            //Makes sure the context has not been disposed 
            IsDisposed();

            //Makes sure the entity is not null
            if (entity == null) throw new ArgumentNullException("entity");

            //Makes sure the entities id is not preset
            if (entity.Id != 0) throw new ArgumentException("Id can't be preset!");

            //Adds the entity to the context
            Db.Add(entity);

            //Saves the context
            SaveChanges();

            //Checks that the id has been set
            if (entity.Id == 0) throw new InternalDbException("Id was not set");

        }

        /// <summary>
        /// Puts the given entity to the database.
        /// This means that the currently DB stored entity will be overridden with the given entity. The match is made on ID's
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entity">The new version of the entity</param>
        /// <remarks>
        /// @pre !IsDisposed();
        /// @pre entity != null
        /// @pre entity.Id > 0
        /// @pre int.MaxValue >= entity.Id
        /// </remarks>
        public override void Update<TEntity>(TEntity entity)
        {
            IsDisposed(); //Checks that the context is not disposed
            if (entity == null){ throw new ArgumentNullException("entity");} //Checks that the entity is not null
            if (entity.Id <= 0){ throw new InternalDbException("Id was zero or below");}
            if (entity.Id > int.MaxValue){ throw new InternalDbException("Id was larger than int.MaxValue");}
            Db.Update(entity); //Updates the entity
            SaveChanges(); //Saves the changes to the context
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <remarks>
        /// @pre !IsDisposed();
        /// @pre entity != null
        /// @pre entity.Id > 0
        /// @pre int.MaxValue >= entity.Id
        /// </remarks>
        public override void Delete<TEntity>(TEntity entity)
        {
            IsDisposed(); //Checks that the context is not disposed
            if (entity == null){ throw new ArgumentNullException("entity"); } //Checks that the entity is not null
            if (entity.Id <= 0) throw new InternalDbException("Id was zero or below");
            if (entity.Id > int.MaxValue) throw new InternalDbException("Id was zero or below");
            Db.Delete(entity); //Deletes the entity
            SaveChanges(); //Saves the changes to the database
        }

        /// <summary>
        /// Deletes the given entity from the data
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <remarks>
        /// @pre !IsDisposed();
        /// @pre id >= 0
        /// @pre int.MaxValue > id
        /// </remarks>
        public override void Delete<TEntity>(int id)
        {
            if (id <= 0) { throw new ArgumentException("Ids 0 or less"); } //Checks that the id is not 0 or less
            if (id > int.MaxValue) { throw new ArgumentException("Ids larger than int.MaxValue"); } //Checks that the id is not more that int.maxValue
            Delete(Get<TEntity>(id)); //Calls Delete with entity fetched by id
        }
    }
}
