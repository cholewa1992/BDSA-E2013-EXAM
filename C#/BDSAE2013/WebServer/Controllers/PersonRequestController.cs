using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public class PersonRequestController : AbstractRequestController
    {
        public override string Keyword { get; set; }

        public PersonRequestController()
        {
            Keyword = "Person";
        }

        public override void ProcessGet(Request request)
        {
            Console.WriteLine("Person Get");
        }

        public override void ProcessPut(Request request)
        {
            Console.WriteLine("Person Put");
        }

        public override void ProcessPost(Request request)
        {
            Console.WriteLine("Person Post");
        }

        public override void ProcessDelete(Request request)
        {
            Console.WriteLine("Person Delete");
        }
    }
}
