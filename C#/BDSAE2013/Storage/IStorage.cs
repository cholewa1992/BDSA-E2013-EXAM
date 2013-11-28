using System;
using Storage.EntityDto;

namespace Storage
{
    public interface IStorage : IDisposable
    {
        TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto, new();
        bool SaveChanges();
        bool Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new();
        bool Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new();
        bool Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new();
    }
}
