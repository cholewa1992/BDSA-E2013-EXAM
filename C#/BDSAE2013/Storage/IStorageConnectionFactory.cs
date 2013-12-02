namespace Storage
{
    /// <summary>
    /// Abstractfactory implementation for creating storage connections
    /// </summary>
    public interface IStorageConnectionFactory
    {
        /// <summary>
        /// Creates a connection
        /// </summary>
        /// <returns>the active IStorageConnection connection</returns>
        IStorageConnection GetConnection();
    }
}
