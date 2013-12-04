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

            Request request = new Request();
            request.Method = "GET http://localhost:112/Movie/5";

            Request response = delegator.ProcessRequest(request);

            //Console.WriteLine(JSonParser.GetValues(Encoder.Decode(response.Data))["title"]);
            Console.WriteLine(response.ResponseStatusCode);
            Console.WriteLine(JSonParser.GetValues(Encoder.Decode(response.Data))["response"]);

            Console.ReadKey();

            //RequestDelegator delegator = new RequestDelegator();

            //Request request = new Request();

            ////Test movie processing
            //request.Method = "GET http://localhost:112/Search/Lord";
            //Request response = delegator.ProcessRequest(request);

            //string json = Encoder.Decode(response.Data);

            //Dictionary<string, string> values = JSonParser.GetValues(json);

            //int index = 0;

            //while (values.ContainsKey("m" + index + "Id"))
            //{
            //    Console.WriteLine(values["m" + index + "Id"] + ": " + values["m" + index + "Title"]); ;

            //    index++;
            //}

            //index = 0;

            //while (values.ContainsKey("p" + index + "Id"))
            //{
            //    Console.WriteLine(values["p" + index + "Id"] + ": " + values["p" + index + "Name"]); ;

            //    index++;
            //}

            //Console.WriteLine("Finished");
            //Console.ReadKey();



            //            Request request = new Request();

            //            //Test movie processing
            //            request.Method = "GET http://localhost:112/Movie/5";
            //            delegator.ProcessRequest(request);

            //            request.Method = "POST http://localhost:112/Movie";
            //            request.Data = Encoding.GetEncoding("iso-8859-1").GetBytes(JSonParser.Parse(
            //                "title", "Die Hard",
            //                "kind", "Drama",
            //                "year", "1992",
            //                "seasonNumber", "0",
            //                "episodeNumber", "0",
            //                "seriesYear", "0",
            //                "episodeOfId", "0"));
            //            delegator.ProcessRequest(request);

            //            /*
            //            request.Method = "PUT http://localhost:112/Movie";
            //            delegator.ProcessRequest(request);

            //            request.Method = "DELETE http://localhost:112/Movie";
            //            delegator.ProcessRequest(request);

            //            //Test user processing
            //            request.Method = "GET http://localhost:112/User";
            //            delegator.ProcessRequest(request);

            //            request.Method = "POST http://localhost:112/User";
            //            delegator.ProcessRequest(request);

            //            request.Method = "PUT http://localhost:112/User";
            //            delegator.ProcessRequest(request);

            //            request.Method = "DELETE http://localhost:112/User";
            //            delegator.ProcessRequest(request);

            //            //Test Person processing

            //            request.Method = "GET http://localhost:112/Person";
            //            delegator.ProcessRequest(request);

            //            request.Method = "POST http://localhost:112/Person";
            //            delegator.ProcessRequest(request);

            //            request.Method = "PUT http://localhost:112/Person";
            //            delegator.ProcessRequest(request);

            //            request.Method = "DELETE http://localhost:112/Person";
            //            delegator.ProcessRequest(request);

            //            //Test Favourite processing
            //            request.Method = "GET http://localhost:112/Favourite";
            //            delegator.ProcessRequest(request);

            //            request.Method = "POST http://localhost:112/Favourite";
            //            delegator.ProcessRequest(request);

            //            request.Method = "PUT http://localhost:112/Favourite";
            //            delegator.ProcessRequest(request);

            //            request.Method = "DELETE http://localhost:112/Favourite";
            //            delegator.ProcessRequest(request);

            //             * */
            //            Console.ReadKey();
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
            using( var rd = new RequestDelegator( new StorageConnectionBridgeFacade( new EFConnectionFactory() ) ) )
            {
                var result = rd.ProcessRequest( request );

                ch.RespondToRequest( result );
            }
        }
    }
}