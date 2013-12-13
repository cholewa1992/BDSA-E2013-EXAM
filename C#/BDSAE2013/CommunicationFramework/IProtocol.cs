using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    
    /// <summary>
    /// Interface for all the protocols to adhere to
    /// </summary>
    public interface IProtocol
    {

        /// <summary>
        /// Storing the address of the second party in the communication
        /// </summary>
        string Address{ get; set; }


        /// <summary>
        /// The purpose of this method is to receive a response to an earlier sent message, 
        /// using method SendMessage.
        /// </summary>
        /// <param name="timeToWaitFor">Amount of miliseconds before timeToWaitFor</param>
        /// <returns>The received data</returns>
        byte[] GetResponse( int timeToWaitFor );

        /// <summary>
        /// The purpose of this method is to send a package to an Address
        /// </summary>
        /// <param name="data">Byte array representation of data to be sent</param>
        /// <param name="method">Header for the data to be sent</param>
        void SendMessage( byte[] data, string method );

        /// <summary>
        /// The purpose of this method is to return a request object for 
        /// an incoming request on a specified protocol.
        /// </summary>
        /// <returns>A request object created from the request that was received using the protocol</returns>
        Request GetRequest();


        /// <summary>
        /// The purpose of this method is to respond to an earlier received request, back
        /// to whoever sent it
        /// </summary>
        /// <param name="request">A request object to respond with</param>
        void RespondToRequest( Request request );
    }
}