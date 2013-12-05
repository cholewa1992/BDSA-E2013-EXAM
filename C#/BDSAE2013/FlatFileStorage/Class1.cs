using System;
using System.Linq;
using Storage;

namespace FlatFileStorage
{
    public class FlatFileStorageConnection : IStorageConnection
    {
        public void Dispose()
        {
            throw new NotImplementedException();
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
