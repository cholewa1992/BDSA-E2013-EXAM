using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    public class UserRequestController : AbstractRequestController
    {
        public UserRequestController()
        {
            Keyword = "User";
        }

        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            Console.WriteLine("User Get");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            Console.WriteLine("User Put");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            Console.WriteLine("User Post");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            Console.WriteLine("User Delete");
            return (storage => "Not Yet Implemented");
        }
    }
}