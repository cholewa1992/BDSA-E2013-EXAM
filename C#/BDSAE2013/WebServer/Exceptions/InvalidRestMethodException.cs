using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class InvalidRestMethodException : ArgumentException
    {
        public InvalidRestMethodException() : base() { }
        public InvalidRestMethodException(string message) : base(message) { }
        public InvalidRestMethodException(string message, System.Exception inner) : base(message, inner) { }
    }
}
