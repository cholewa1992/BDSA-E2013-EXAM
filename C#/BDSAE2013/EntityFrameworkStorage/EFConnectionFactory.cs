
using Storage;

namespace EntityFrameworkStorage
{
    /// <summary>
    /// Concret implementation of IStorageConnectionFactory for creating RDBMS storage connections
    /// </summary>
    public class EFConnectionFactory : IStorageConnectionFactory
    {
        /// <summary>
        /// Creates a FakeImdbContext connection
        /// </summary>
        /// <returns>The active IStorageConnection connection</returns>
        public IStorageConnection CreateConnection()
        {
            return new EFStorageConnection<FakeImdbContext>();
        }

        /// <summary>
        /// Creates an RDBMS connection
        /// </summary>
        /// <typeparam name="TContext">The context to use for the connection</typeparam>
        /// <returns>The active IStorageConnection connection</returns>
        public IStorageConnection GetConnection<TContext>() where TContext : IDbContext, new()
        {
            return new EFStorageConnection<TContext>();
        }
    }
}
