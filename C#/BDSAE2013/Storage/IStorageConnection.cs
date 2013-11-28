using System;
using System.Linq;

namespace Storage
{
    public interface IStorageConnection : IDisposable
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto;
        TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto;
        bool SaveChanges();
        void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto;
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto;
    }
}
