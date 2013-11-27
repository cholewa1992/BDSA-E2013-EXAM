using System;
using System.Collections.Generic;
using Storage.EntityDto;

namespace Storage
{
    public interface IStorage : IDisposable
    {
        TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto, new();
        bool SaveChanges();
        TEntity Add<TEntity>(TEntity entity) where TEntity : IEntityDto;
        TEntity Update<TEntity>(TEntity entity) where TEntity : IEntityDto;
        bool Delete<TEntity>(TEntity entity) where TEntity : IEntityDto;
    }
}
