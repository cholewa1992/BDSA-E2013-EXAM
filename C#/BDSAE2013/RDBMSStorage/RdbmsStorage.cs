using System;
using System.Collections.Specialized;
using System.Linq;
using Storage;
using Storage.EntityDto;

namespace RDBMSStorage
{
    class RdbmsStorage : IStorage
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TEntity Get<TEntity>(int id) where TEntity : class, IEntityDto, new()
        {
            if (typeof (TEntity) == typeof (MovieDto))
            {
                using (var con = new fakeimdbEntities())
                {
                    var o = con.Movies.Single(m => m.Id == id);
                    Console.WriteLine(con.Movies.Count());
                    return DtoTransformer.Transform<TEntity>(o);
                }

            }
            throw new InvalidOperationException(typeof(TEntity) + " is not implemented as an entity type");
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class, IEntityDto, new()
        {
            throw new NotImplementedException();
        }
    }
}
