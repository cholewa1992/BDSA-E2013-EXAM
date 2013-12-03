using System;
namespace Storage
{
    public class InternalDbException : Exception
    {
        private readonly string _m;

        public override string Message
        {
            get { return _m; }
        }
        public Exception UnderlyingException { get; private set; }

        public InternalDbException(string message, Exception underlyingException = null)
        {
            _m = message;
            UnderlyingException = underlyingException;
        }

        public override string ToString()
        {
            return _m;
        }
    }
}
