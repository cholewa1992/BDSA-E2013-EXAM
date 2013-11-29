using System;
using System.Data;
using System.Linq;
using Storage;
using EntityState = System.Data.EntityState;

namespace EntityFrameworkStorage
{
    class EFStorageConnection<TContext> : IStorageConnection where TContext : IDbContext, new()
    {

        private readonly TContext _ef;

        public EFStorageConnection()
        {
            _ef = new TContext();
        } 
        public void Dispose()
        {
            _ef.Dispose();
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntityDto
        {
            return _ef.Set<TEntity>();
        }

        public TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto
        {
            return _ef.Set<TEntity>().Single(t => t.Id == id);
        }

        public bool SaveChanges()
        {
            return _ef.SaveChanges() > 0;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            if(entity.Id != 0) throw new InvalidOperationException("The Id of an new entity should not be set!");
            try
            {
                entity.Id = Get<TEntity>().Max(t => t.Id) + 1;
            }
            catch(Exception)
            {
                entity.Id = 1;
            }
            _ef.Entry(entity).State = EntityState.Added;
            _ef.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            if (entity.Id == 0)
            {
                Add(entity);
                return;
            }
            _ef.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto
        {
            _ef.Entry(entity).State = EntityState.Deleted;
        }
    }
}
