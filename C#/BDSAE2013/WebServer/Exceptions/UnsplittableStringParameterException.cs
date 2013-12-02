using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class UnsplittableStringParameterException : ArgumentException
    {
        public UnsplittableStringParameterException() : base() { }
        public UnsplittableStringParameterException(string message) : base(message) { }
        public UnsplittableStringParameterException(string message, System.Exception inner) : base(message, inner) { }
    }
}
