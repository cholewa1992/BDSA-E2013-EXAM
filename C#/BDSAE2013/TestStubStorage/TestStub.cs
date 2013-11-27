using System;
using System.Collections.Generic;
using System.IO;
using Storage;
using FlatFileStorage;
using Storage.EntityDto;

namespace TestStubStorage
{
    /// <summary>
    /// TestStub. Only to use for TestPurposes! The module is not persistence and will erase it self when disposed
    /// </summary>
    public class TestStub : IStorage
    {
        private readonly IStorage _storage;

        /// <summary>
        /// Creates a new instance of Teststub
        /// </summary>
        internal TestStub()
        {
            _storage = new FileStorageFactory("testdata.dat").GetConnection();
        }

        public TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves changes of the entities to the data file
        /// </summary>
        /// <returns>returns true if the operation was succesfull</returns>
        public bool SaveChanges()
        {
            return _storage.SaveChanges();
        }

        /// <summary>
        /// Adds a new entity to the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to add</typeparam>
        /// <param name="entity">The entity to add</param>
        /// <returns>The just added entity</returns>
        public TEntity Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            return _storage.Add(entity);
        }

        /// <summary>
        /// updates an entity in the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to update</param>
        /// <returns>The just updated entity</returns>
        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            return _storage.Update(entity);
        }

        /// <summary>
        /// Delete an entity in the storage.
        /// The data is not finally saves before SaveData is called
        /// </summary>
        /// <typeparam name="TEntity">The entity type to use</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if operation was successful</returns>
        public bool Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            return _storage.Delete(entity);
        }

        /// <summary>
        /// Disposes the storage and deletes it! It's not recoverable
        /// </summary>
        public void Dispose()
        {
            _storage.Dispose();
            try
            {
                File.Delete("testdata.dat");
            }
            catch (IOException e)
            {
                #if DEBUG
                    Console.WriteLine("testdata.dat");
                    Console.WriteLine(@"Test stub couldt not be removed!: " + e);
                #else
                    throw new InvalidOperationException("Test stub couldt not be removed!");
                #endif
            }

            GC.SuppressFinalize(this);
        }
    }
}
