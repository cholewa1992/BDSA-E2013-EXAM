using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class StorageNullException : Exception
    {
        public StorageNullException() : base() { }
        public StorageNullException(string message) : base(message) { }
        public StorageNullException(string message, System.Exception inner) : base(message, inner) { }
    }
}
