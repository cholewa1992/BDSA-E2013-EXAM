using System;

namespace CommunicationFramework
{
    public enum Protocols
    {
        Http
    }

    public class CommunicationHandler
    {
        private IProtocol _protocolInstance;

        private IProtocol ProtocolInstance
        {
            get { return _protocolInstance ?? ( _protocolInstance = getProtocol( Protocol ) ); }
        }

        /// <summary>
        /// Initialize an instance of the protocol based on the enum value
        /// 
        /// @post protocolInstance != null
        /// </summary>
        /// <param name="protocol">Enum of the protocol</param>
        /// <returns>An instance of the protocol</returns>
        private IProtocol getProtocol( Protocols protocol )
        {
            try
            {
                //Find the class in the current assembly and create an instance of it
                IProtocol protocolInstance = (IProtocol) Activator.CreateInstance( null, "CommunicationFramework." + protocol.ToString() + "Protocol" ).Unwrap();
                //CheckPostCondition protocolInstance != null
                if( protocolInstance == null )
                    throw new Exception();

                return protocolInstance;
            }
            catch
            {
                //Incase an error happens, we return a default HTTPProtocol because the protocol cannot be null
                return new HTTPProtocol();
            }
        }

        private Protocols _protocol;
        public Protocols Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                if( !Enum.IsDefined( typeof( Protocols ), value ) )
                    throw new ProtocolException( "ERROR! Supplied protocol does not exist" );

                _protocol = value;
            }
        }


        /// <summary>
        /// Constructor for CommunicationHandler
        /// </summary>
        /// <param name="protocol">Enum of the protocol</param>
        public CommunicationHandler( Protocols protocol )
        {
            Protocol = protocol;
        }

        /// <summary>
        /// Send a request using the protocol specified
        /// 
        /// @pre address != null
        /// @pre address != ""
        /// @pre data != null
        /// @pre method != null
        /// @pre method != ""
        /// </summary>
        /// <param name="address">Address to send the request to. Cannot be null</param>
        /// <param name="data">Data of the request. Cannot be null</param>
        /// <param name="method">Method of the request, such as: "GET", "POST", "PUT, "DELETE". Cannot be null or empty</param>
        public void Send( string address, byte[] data, string method )
        {
            //CheckPreCondition address != null
            //CheckPreCondition address != ""
            if( String.IsNullOrEmpty( address ) )
                throw new ProtocolException( "ERROR! Address cannot be null or empty" );
            //CheckPreCondition method != null
            //CheckPreCondition method != ""
            if( String.IsNullOrEmpty( method ) )
                throw new ProtocolException( "ERROR! Method cannot be null or empty" );
            //CheckPreCondition data != null
            if( data == null && method.ToLower() != "get" )
                throw new ProtocolException( "ERROR! Data cannot be null for any methods other than GET" );

            ProtocolInstance.Address = address;
            ProtocolInstance.SendMessage( data, method );
        }

        /// <summary>
        /// Receive a request from the protocol, implying a request was already sent. After the timeout has passed, will cast a ProtocolException
        /// </summary>
        /// <param name="timeout">Amount of time to wait for the response</param>
        /// <returns>The data from the response received</returns>
        public byte[] Receive( int timeout = 10000 )
        {
            return ProtocolInstance.GetResponse( timeout );
        }

        /// <summary>
        /// Await requests sent to the address specified or the default address incase it wasn't set
        /// </summary>
        /// <returns>A request object per request received</returns>
        public Request GetRequest( String address )
        {
            ProtocolInstance.Address = address;

            return ProtocolInstance.GetRequest();
        }

        /// <summary>
        /// Respond to a receive request based on the request object
        /// 
        /// @pre request != null
        /// @pre request.data != null
        /// </summary>
        /// <param name="request">The request object to respond to. Cannot be null and must be the same as was received by the GetRequest method.</param>
        public void RespondToRequest( Request request )
        {
            //CheckPreCondition Protocol != null
            if( request == null )
                throw new ProtocolException( "ERROR! Request cannot be null" );
            //CheckPreCondition Protocol != null
            if( request.Data == null )
                throw new ProtocolException( "ERROR! Data cannot be null" );

            ProtocolInstance.RespondToRequest( request );
        }
    }
}