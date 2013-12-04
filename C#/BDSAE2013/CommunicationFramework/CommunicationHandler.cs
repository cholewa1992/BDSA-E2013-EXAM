using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    public enum Protocols
    {
        HTTP
    }

    public class CommunicationHandler
    {
        private IProtocol _protocolInstance;
        private IProtocol ProtocolInstance
        {
            get
            {
                if( _protocolInstance == null )
                    _protocolInstance = getProtocol( Protocol );

                return _protocolInstance;
            }
            set { _protocolInstance = value; }
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

        public Protocols Protocol{ get; set; }


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
        /// @pre Protocol != null
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
            //CheckPreCondition Protocol != null
            if( Protocol == null )
                throw new ProtocolException( "ERROR! Protocol not set" );
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
                throw new ProtocolException( "ERROR! Data cannot be null" );

            ProtocolInstance.Address = address;
            ProtocolInstance.SendMessage( data, method );
        }

        /// <summary>
        /// Receive a request from the protocol, implying a request was already sent. After the timeout has passed, will cast a ProtocolException
        /// 
        /// @pre protocol != null
        /// </summary>
        /// <param name="timeout">Amount of time to wait for the response</param>
        /// <returns>The data from the response received</returns>
        public byte[] Receive( int timeout )
        {
            //CheckPreCondition protocol != null
            if( Protocol == null )
                throw new ProtocolException( "ERROR! Protocol not set" );

            return ProtocolInstance.GetResponse( timeout );
        }

        /// <summary>
        /// Await requests sent to the address specified or the default address incase it wasn't set
        /// 
        /// @pre Protocol != null
        /// </summary>
        /// <returns>A request object per request received</returns>
        public Request GetRequest(String address)
        {
            //CheckPreCondition Protocol != null
            if( Protocol == null )
                throw new ProtocolException( "ERROR! Protocol not set" );

            ProtocolInstance.Address = address;

            return ProtocolInstance.GetRequest();
        }

        /// <summary>
        /// Respond to a receive request based on the request object
        /// 
        /// @pre Protocol != null
        /// @pre request != null
        /// @pre request.data != null
        /// </summary>
        /// <param name="request">The request object to respond to. Cannot be null and must be the same as was received by the GetRequest method.</param>
        public void RespondToRequest( Request request )
        {
            //CheckPreCondition Protocol != null
            if( Protocol == null )
                throw new ProtocolException( "ERROR! Protocol not set" );
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