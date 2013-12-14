using System;
using System.Threading.Tasks;
using CommunicationFramework;
using EntityFrameworkStorage;
using Storage;

namespace WebServer
{
    /// <summary>
    /// The class used to initialize the server. The server will by default be hosted at http://*:1337/, using a http protocol
    /// The class will listen for incoming requests and parse them on to the request delegator.
    /// The class will respond with any error code and data given by the request delegator
    /// </summary>
    public class WebServer
    {
        public static void Main( String[] args )
        {
            try
            {
                //Initialize a new server
                new WebServer().Start("http://*:1337/", Protocols.Http);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start server " + e);
            }
        }

        /// <summary>
        /// Starts the server and listen for requests
        /// </summary>
        /// <param name="listenAddress"> The address to listen at </param>
        /// <param name="protocol"> The protocol to use </param>
        public void Start(String listenAddress, Protocols protocol)
        {
            Console.WriteLine( "Server started listening on " + listenAddress );
            var communicationHandler = new CommunicationHandler( protocol );
            while( true )
            {
                //Listen for incoming requests and start a new thread for each of them
                var request = communicationHandler.GetRequest( listenAddress );
                Task.Run( () => StartRequestDelegatorThread( request, communicationHandler ) );

                Console.WriteLine( "new thread started" );
            }
        }

        /// <summary>
        /// The main thread for each incoming requests
        /// </summary>
        /// <param name="request"> The request that has been received </param>
        /// <param name="communicationHandler"> The communication handler to respond to </param>
        public void StartRequestDelegatorThread( Request request, CommunicationHandler communicationHandler )
        {
            using (var requestDelegator = new RequestDelegator(new StorageConnectionBridgeFacade(new EFConnectionFactory<fakeimdbEntities>())))
            {
                try
                {
                    var result = requestDelegator.ProcessRequest(request);

                    communicationHandler.RespondToRequest(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}