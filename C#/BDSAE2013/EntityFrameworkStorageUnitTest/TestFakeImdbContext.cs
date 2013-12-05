using System.Data.Entity;
using EntityFrameworkStorage;
using Storage;

namespace EntityFrameworkStorageUnitTest
{
    class MockFakeImdbContext : DbContext, IDbContext
    {
        public MockFakeImdbContext()
        {
            Database.ExecuteSqlCommand("TRUNCATE TABLE UserAccs;");
        }

        public IDbSet<UserAcc> UserAcc { set; get; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}