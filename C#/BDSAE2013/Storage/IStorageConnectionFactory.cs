namespace Storage
{
    /// <summary>
    /// Abstractfactory implementation for creating storage connections
    /// </summary>
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public interface IStorageConnectionFactory
    {
        /// <summary>
        /// Creates a connection
        /// </summary>
        /// <returns>the active IStorageConnection connection</returns>
        IStorageConnection CreateConnection();
    }
}
