using System.Net;
using Mankey_Pidgey_AS39;
using Storage;

namespace EntityFrameworkStorage
{
    public class EFConnectionFactory : IStorageConnectionFactory
    {
        public IStorageConnection GetConnection()
        {
            return new EFStorageConnection<FakeImdbContext>();
        }

        public IStorageConnection GetConnection<TContext>() where TContext : IDbContext, new()
        {
            return new EFStorageConnection<TContext>();
        }
    }
}
