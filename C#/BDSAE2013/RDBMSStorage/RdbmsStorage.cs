using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IList<TEntity> Get<TEntity>() where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }

        public TEntity Get<TEntity>(int id) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : IEntityDto
        {
            throw new NotImplementedException();
        }
    }
}
