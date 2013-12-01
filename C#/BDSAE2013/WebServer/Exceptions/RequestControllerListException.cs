using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class RequestControllerListException : Exception
    {
        public RequestControllerListException() : base() { }
        public RequestControllerListException(string message) : base(message) { }
        public RequestControllerListException(string message, System.Exception inner) : base(message, inner) { }
    }
}
