namespace Storage
{
    public interface IStorageConnectionFactory
    {
        IStorageConnection GetConnection();
    }
}
