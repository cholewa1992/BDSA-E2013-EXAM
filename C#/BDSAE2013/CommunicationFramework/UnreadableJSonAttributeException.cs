using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    public class UnreadableJSonAttributeException : Exception
    {
        public UnreadableJSonAttributeException() : base() { }
        public UnreadableJSonAttributeException(string message) : base(message) { }
        public UnreadableJSonAttributeException(string message, System.Exception inner) : base(message, inner) { }
    }
}
