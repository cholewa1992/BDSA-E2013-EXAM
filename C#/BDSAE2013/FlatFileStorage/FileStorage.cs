using System;
using System.Collections;
using System.Collections.Generic;
using Storage;
using Storage.EntityDto;

namespace FlatFileStorage
{
    /// <summary>
    /// FilesStorage is a local file persistence module
    /// </summary>
    public class FileStorage : IStorage
    {
        private readonly Dictionary<Type, IList> _data;
        private readonly CustomFileStream _customeFileStream;

        public string FilePath
        {
            get { return _customeFileStream.FilePath; }
        }

        /// <summary>
        /// Initializes a new FileStorage
        /// </summary>
        internal FileStorage(CustomFileStream customeFileStream)
        {
            _customeFileStream = customeFileStream;
            if (_customeFileStream == null) throw new ArgumentNullException("customeFileStream");
            _data = customeFileStream.Load();
        }


        /// <summary>
        /// Returns a list of entities
        /// </summary>
        /// <typeparam name="TEntity">Entity type to query for</typeparam>
        /// <returns>An IList of the entities</returns>
        public IList<TEntity> Get<TEntity>() where TEntity : IEntityDto
        {
            return GetTypeList<TEntity>();
        }

        public TEntity Get<TEntity>(int id) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Private method to get a list of entities, or make one if it's not already in the data dictionary 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private List<TEntity> GetTypeList<TEntity>() where TEntity : IEntityDto
        {
            if (!_data.ContainsKey(typeof (TEntity)))
            {
                _data.Add(
                    typeof (TEntity),
                    (IList) Activator.CreateInstance(
                        typeof (List<>).MakeGenericType(
                            typeof (TEntity)))
                    );
            }
            return (List<TEntity>) _data[typeof (TEntity)];

        }


        /// <summary>
        /// Saves changes of the entities to the data file
        /// </summary>
        /// <returns>returns true if the operation was succesfull</returns>
        public bool SaveChanges()
        {
            return _customeFileStream.SaveChanges(_data);
        }

        /// <summary>
        /// Adds a new entity to the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add</param>
        /// <returns>The just added entity</returns>
        public TEntity Add<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            Get<TEntity>().Add(entity);
            return entity;
        }

        /// <summary>
        /// updates an entity in the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to update</param>
        /// <returns>The just updated entity</returns>
        public TEntity Update<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an entity in the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if operation was successful</returns>
        public bool Delete<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            return Get<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Disposes the FileStoragee corretly by making sure all data is saved before closure
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
