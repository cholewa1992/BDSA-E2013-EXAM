
using Storage;

namespace EntityFrameworkStorage
{
    /// <summary>
    /// Concret implementation of IStorageConnectionFactory for creating RDBMS storage connections
    /// </summary>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class EFConnectionFactory<TContext> : IStorageConnectionFactory where TContext : IDbContext, new()
    {
        /// <summary>
        /// Creates an RDBMS connection
        /// </summary>
        /// <typeparam name="TContext">The context to use for the connection</typeparam>
        /// <returns>The active IStorageConnection connection</returns>
        public IStorageConnection CreateConnection()
        {
            return new EFStorageConnection<TContext>();
        }
    }
}
