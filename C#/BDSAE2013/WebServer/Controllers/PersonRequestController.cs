using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    public class PersonRequestController : AbstractRequestController
    {
        public PersonRequestController()
        {
            Keyword = "Person";
        }

        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            Console.WriteLine("Person Get");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            Console.WriteLine("Person Put");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            Console.WriteLine("Person Post");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            Console.WriteLine("Person Delete");
            return (storage => "Not Yet Implemented");
        }
    }
}
