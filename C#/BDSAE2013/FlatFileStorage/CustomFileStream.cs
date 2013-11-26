using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlatFileStorage
{
    public class CustomFileStream : ICustomFileStream
    {
        public string FilePath { get; set; }
        /// <summary>
        /// Initializes a new FileStorage. The data file will be created in the install directory 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="filePath"></param>
        internal CustomFileStream(string filename = "data.dat", string filePath = "")
        {
            FilePath = filePath != String.Empty ? filePath + @"\" + filename : filename;
        }

        /// <summary>
        /// Loads all entity lists from the data file
        /// </summary>
        /// <returns>A Dictionary containg the list of given entity types</returns>
        public Dictionary<Type, IList> Load()
        {
            try
            {
                Stream stream = File.Open(FilePath, FileMode.Open);
                var bformatter = new BinaryFormatter();
                var data = (Dictionary<Type, IList>)bformatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            catch (SerializationException)
            {
                Console.WriteLine(@"Not loaded correctly");
            }
            catch (FileNotFoundException) { }
            return new Dictionary<Type, IList>();
        }

        /// <summary>
        /// Saves changes of the entities to the data file
        /// </summary>
        /// <returns>returns true if the operation was succesfull</returns>
        public bool SaveChanges(Dictionary<Type, IList> data)
        {
            try
            {
                Stream stream = File.Open(FilePath, FileMode.Create);
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, data);
                stream.Flush();
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
