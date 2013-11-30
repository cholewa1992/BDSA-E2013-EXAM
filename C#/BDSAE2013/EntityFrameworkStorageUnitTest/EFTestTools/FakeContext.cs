using System;
using System.Data.Entity;
using EntityFrameworkStorage;
using Storage;

namespace EntityFrameworkStorageUnitTest.EFTestTools
{
    class FakeContext : DbContext, IDbContext
    {
        public IDbSet<UserAcc> UserAcc { set; get; }
        public IDbSet<Movies> Movies { set; get; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
