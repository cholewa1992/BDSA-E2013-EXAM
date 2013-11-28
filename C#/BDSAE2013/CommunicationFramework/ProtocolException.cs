using System;
using System.Runtime.Serialization;

namespace CommunicationFramework
{
    public class ProtocolException : Exception
    {
        public ProtocolException()
        {
            // Add implementation.
        }

        public ProtocolException(string message)
            : base(message)
        {
            
        }

        public ProtocolException(string message, Exception inner) 
            : base(message, inner)
        {
            
        }

        // This constructor is needed for serialization.
        protected ProtocolException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    }
}