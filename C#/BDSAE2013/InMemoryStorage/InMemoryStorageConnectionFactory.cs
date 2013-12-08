using Storage;

namespace InMemoryStorage
{
    public class InMemoryStorageConnectionFactory : IStorageConnectionFactory
    {
        public IStorageConnection CreateConnection()
        {
            return new InMemoryStorageConnection();
        }
    }
}
