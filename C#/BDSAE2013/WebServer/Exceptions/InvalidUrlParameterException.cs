using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class InvalidUrlParameterException : ArgumentException
    {
        public InvalidUrlParameterException() : base() { }
        public InvalidUrlParameterException(string message) : base(message) { }
        public InvalidUrlParameterException(string message, System.Exception inner) : base(message, inner) { }
    }
}
