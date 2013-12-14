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
            try
            {
                new WebServer().Start("http://*:1337/", Protocols.Http);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start server " + e);
            }
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

                try
                {
                    ch.RespondToRequest(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}