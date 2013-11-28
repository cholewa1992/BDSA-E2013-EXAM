using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace RDBMSStorage
{
    public interface IDbContext : IDisposable
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        DbEntityEntry Entry(object o);
    }
}
