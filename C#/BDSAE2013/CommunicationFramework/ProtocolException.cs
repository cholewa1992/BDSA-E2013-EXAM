using System;
using System.Runtime.Serialization;

namespace CommunicationFramework
{

    /// <author>
    /// Mathias Pedersen (mkin@itu.dk)
    /// Martin Juul Petersen (mjup@itu.dk)
    /// </author>
    public class ProtocolException : Exception
    {
        public ProtocolException()
        {
            // Add implementation.
        }

        public ProtocolException( string message ) : base( message )
        {

        }

        public ProtocolException( string message, Exception inner ) : base( message, inner )
        {

        }

        // This constructor is needed for serialization.
        protected ProtocolException( SerializationInfo info, StreamingContext context )
        {
            // Add implementation.
        }
    }
}