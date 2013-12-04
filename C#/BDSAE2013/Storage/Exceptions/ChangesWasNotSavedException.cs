using System;

namespace Storage
{
    public class ChangesWasNotSavedException : Exception
    {
        public ChangesWasNotSavedException(){}
        public ChangesWasNotSavedException(string message) : base(message) { }
        public ChangesWasNotSavedException(string message, Exception inner) : base(message, inner) { }
    }
}
