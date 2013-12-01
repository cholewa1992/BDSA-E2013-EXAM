using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class KeywordNullException : Exception
    {
        public KeywordNullException() : base() { }
        public KeywordNullException(string message) : base(message) { }
        public KeywordNullException(string message, System.Exception inner) : base(message, inner) { }
    }
}
