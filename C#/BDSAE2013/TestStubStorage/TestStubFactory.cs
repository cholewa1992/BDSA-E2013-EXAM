using Storage;

namespace TestStubStorage
{
    /// <summary>
    /// Factory for initializing instances of TestStub
    /// </summary>
    public class TestStubFactory : IStorageFactory
    {
        /// <summary>
        /// Initializes a new instance of TestStub
        /// </summary>
        /// <returns>The persistance module of type IUCalStorage</returns>
        public IStorage GetConnection()
        {
            return new TestStub();
        }
    }
}
