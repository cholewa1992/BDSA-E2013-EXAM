using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public abstract class AbstractRequestController : IRequestController
    {
        public abstract string Keyword { get; set; }

        public void ProcessRequest(Request request)
        {
            string input = request.Method.Split(' ')[0];

            switch (input) 
            { 
                case "GET":
                    ProcessGet(request);
                    break;

                case "POST":
                    ProcessPost(request);
                    break;

                case "DELETE":
                    ProcessDelete(request);
                    break;

                case "PUT":
                    ProcessPut(request);
                    break;


            }
        }


        public abstract void ProcessGet(Request request);
        public abstract void ProcessPost(Request request);
        public abstract void ProcessDelete(Request request);
        public abstract void ProcessPut(Request request);


    }
}
