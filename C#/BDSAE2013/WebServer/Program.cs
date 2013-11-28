using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestDelegator delegator = new RequestDelegator();

            Request request = new Request();

            //Test movie processing
            request.Method = "GET http://localhost:112/Movie";
            delegator.ProcessRequest(request);

            request.Method = "POST http://localhost:112/Movie";
            delegator.ProcessRequest(request);

            request.Method = "PUT http://localhost:112/Movie";
            delegator.ProcessRequest(request);

            request.Method = "DELETE http://localhost:112/Movie";
            delegator.ProcessRequest(request);

            //Test user processing
            request.Method = "GET http://localhost:112/User";
            delegator.ProcessRequest(request);

            request.Method = "POST http://localhost:112/User";
            delegator.ProcessRequest(request);

            request.Method = "PUT http://localhost:112/User";
            delegator.ProcessRequest(request);

            request.Method = "DELETE http://localhost:112/User";
            delegator.ProcessRequest(request);

            //Test Person processing
            
            request.Method = "GET http://localhost:112/Person";
            delegator.ProcessRequest(request);

            request.Method = "POST http://localhost:112/Person";
            delegator.ProcessRequest(request);

            request.Method = "PUT http://localhost:112/Person";
            delegator.ProcessRequest(request);

            request.Method = "DELETE http://localhost:112/Person";
            delegator.ProcessRequest(request);

            //Test Favourite processing
            request.Method = "GET http://localhost:112/Favourite";
            delegator.ProcessRequest(request);

            request.Method = "POST http://localhost:112/Favourite";
            delegator.ProcessRequest(request);

            request.Method = "PUT http://localhost:112/Favourite";
            delegator.ProcessRequest(request);
            
            request.Method = "DELETE http://localhost:112/Favourite";
            delegator.ProcessRequest(request);
            
            Console.ReadKey();
        }
    }
}
