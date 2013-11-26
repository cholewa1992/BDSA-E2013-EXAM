using Storage;

namespace FlatFileStorage
{
    /// <summary>
    /// Factory to create instances of FileStorage
    /// </summary>
    public class FileStorageFactory : IStorageFactory
    {
        private readonly string _filename;
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of FileStorageFactory
        /// </summary>
        /// <param name="filename">The filename to use</param>
        /// <param name="filePath">The filepath to store data (eg: "C:\Users\[Username]\Desktop")</param>
        public FileStorageFactory(string filename = "data.uCal", string filePath = "")
        {
            _filename = filename;
            _filePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of FileStorage
        /// </summary>
        /// <returns></returns>
        public IStorage GetConnection()
        {
            return new FileStorage(new CustomFileStream(_filename, _filePath));
        }
    }
}
