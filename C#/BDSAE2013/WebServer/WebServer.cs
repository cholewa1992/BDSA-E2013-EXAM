using System;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{


    public class WebServer
    {

        private readonly CommunicationHandler _communicationHandler;


        /*
        public static void Main(String[] args)
        {

            new WebServer("http://localhost:1337", Protocols.HTTP).Start();

        }*/


        public WebServer(String Uri, Protocols protocol)
        {

            _communicationHandler = new CommunicationHandler(protocol);

        }


        public void Start()
        {
            Console.WriteLine("Server started");

            while (true)
            {
                var request = _communicationHandler.GetRequest();
                Task.Run(() => StartRequestDelegatorThread(request));
                Console.WriteLine("new thread started");
            }
        }

        public void StartRequestDelegatorThread(Request request)
        {
            using (var dg = new RequestDelegator())
            {
                dg.ProcessRequest(request);
            }
        }
    }
}