using System;
using System.Threading.Tasks;
using CommunicationFramework;
using EntityFrameworkStorage;
using Storage;
using System.Collections.Generic;
using Utils;

namespace WebServer
{
    public class WebServer
    {
        public static void Main( String[] args )
        {
           // new WebServer().Start( "http://localhost:1337/", Protocols.HTTP );

            RequestDelegator delegator = new RequestDelegator();

            /*Request request = new Request();

            request.Method = "POST http://localhost:112/Movie";
            request.Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard", "year", "1998"));
            */

            /*
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2", "year", "2000", "kind", "TV Series")) };
            Request response = delegator.ProcessRequest(request);

            Console.WriteLine(Encoder.Decode(request.Data));

            Console.WriteLine(JSonParser.GetValues(Encoder.Decode(request.Data))["response"]);

            Console.WriteLine(response.ResponseStatusCode);

            Console.ReadKey();
             */

            
            //Test movie processing
            Request request = new Request(); 
            
            request.Method = "GET http://localhost:112/Search/He";
            Request response = delegator.ProcessRequest(request);

            string json = Encoder.Decode(response.Data);

            Dictionary<string, string> values = JSonParser.GetValues(json);

            int index = 0;

            while (values.ContainsKey("m" + index + "Id"))
            {
                Console.WriteLine(values["m" + index + "Id"] + ": " + values["m" + index + "Title"]); ;

                index++;
            }

            index = 0;

            while (values.ContainsKey("p" + index + "Id"))
            {
                Console.WriteLine(values["p" + index + "Id"] + ": " + values["p" + index + "Name"]); ;

                index++;
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public void Start(String listenAddress, Protocols protocol)
        {
            Console.WriteLine( "Server started listening on " + listenAddress );
            var ch = new CommunicationHandler( protocol );
            while( true )
            {
                var request = ch.GetRequest( listenAddress );

                Task.Run( () => StartRequestDelegatorThread( request, ch ) );

                Console.WriteLine( "new thread started" );
            }
        }

        public void StartRequestDelegatorThread( Request request, CommunicationHandler ch )
        {
            using (var rd = new RequestDelegator(new StorageConnectionBridgeFacade(new EFConnectionFactory<FakeImdbContext>())))
            {
                var result = rd.ProcessRequest( request );

                ch.RespondToRequest( result );
            }
        }
    }
}