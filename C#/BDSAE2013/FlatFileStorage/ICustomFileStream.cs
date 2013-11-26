using System;
using System.Collections;
using System.Collections.Generic;

namespace FlatFileStorage
{
    internal interface ICustomFileStream
    {
        /// <summary>
        /// Loads all entity lists from the data file
        /// </summary>
        /// <returns>A Dictionary containg the list of given entity types</returns>
        Dictionary<Type, IList> Load();

        /// <summary>S
        /// Saves changes of the entities to the data file
        /// </summary>
        /// <returns>returns true if the operation was succesfull</returns>
        bool SaveChanges(Dictionary<Type, IList> data);
    }
}