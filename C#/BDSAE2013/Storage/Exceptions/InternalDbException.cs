using System;
namespace Storage
{
    public class InternalDbException : Exception
    {
        public InternalDbException(){}
        public InternalDbException(string message) : base(message) { }
        public InternalDbException(string message, Exception inner) : base(message, inner) { }
    }
}
