using System;
using System.Data.Entity;
using EntityFrameworkStorage;
using Storage;

namespace EntityFrameworkStorageUnitTest
{
    class FakeContext : DbContext, IDbContext
    {
        public FakeContext()
        {
            Database.ExecuteSqlCommand("TRUNCATE TABLE UserAccs; TRUNCATE TABLE Movies;");
        }

        public IDbSet<UserAcc> UserAcc { set; get; }
        public IDbSet<Movies> Movies { set; get; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}