using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class InvalidServiceRequestException : ArgumentException
    {
        public InvalidServiceRequestException() : base() { }
        public InvalidServiceRequestException(string message) : base(message) { }
        public InvalidServiceRequestException(string message, System.Exception inner) : base(message, inner) { }
    }
}
