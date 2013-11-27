using System;
using System.Collections.Generic;

namespace Storage
{
    public interface IStorage : IDisposable
    {
        IList<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(int id) where TEntity : class;
        bool SaveChanges();
        TEntity Add<TEntity>(TEntity entity) where TEntity : class;
        TEntity Update<TEntity>(TEntity entity) where TEntity : class;
        bool Delete<TEntity>(TEntity entity) where TEntity : class;


    }
}
