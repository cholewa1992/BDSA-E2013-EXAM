using System;
using System.Threading.Tasks;
using CommunicationFramework;
using EntityFrameworkStorage;
using Storage;

namespace WebServer
{
    public class WebServer
    {
        public static void Main( String[] args )
        {
            new WebServer().Start( "http://localhost:1337/", Protocols.HTTP );
        }

        public void Start(String listenAddress, Protocols protocol)
        {
            Console.WriteLine( "Server started listening on " + listenAddress );
            var communicationHandler = new CommunicationHandler( protocol );
            while( true )
            {
                var request = communicationHandler.GetRequest( listenAddress );

                Task.Run( () => StartRequestDelegatorThread( request, communicationHandler ) );

                Console.WriteLine( "new thread started" );
            }
        }

        public void StartRequestDelegatorThread( Request request, CommunicationHandler ch )
        {

            using (var rd = new RequestDelegator(new StorageConnectionBridgeFacade(new EFConnectionFactory<fakeimdbEntities>())))
            {
                var result = rd.ProcessRequest( request );

                ch.RespondToRequest( result );
            }
        }
    }
}