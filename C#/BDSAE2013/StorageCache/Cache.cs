using System;
using System.Collections.Generic;
using System.Linq;
using Storage;

namespace StorageCache
{
    public class Cache : IStorageConnection
    {
        private readonly IStorageConnection _db;

        private static Cache _instance;
        private static IStorageConnectionFactory _factory;

        private readonly Queue<IEntityDto> _localStorage; 

        public static void SetFactory(IStorageConnectionFactory factory)
        {
            if (_factory != null)
            {
                throw new InternalDbException("Factory was already set");
            }
            _factory = factory;
        }

        public static Cache GetInstace()
        {
            if (_factory == null)
            {
                throw new InternalDbException("Cache factory was not set");
            }

            return _instance ?? (_instance = new Cache(_factory));
        }

        private Cache(IStorageConnectionFactory factory)
        {
            _db = factory.CreateConnection();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto
        {
            throw new NotImplementedException();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
