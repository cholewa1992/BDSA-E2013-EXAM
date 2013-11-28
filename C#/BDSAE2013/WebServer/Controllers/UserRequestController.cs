using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public class UserRequestController : AbstractRequestController
    {
        public override string Keyword { get; set; }

        public UserRequestController()
        {
            Keyword = "User";
        }

        public override void ProcessGet(Request request)
        {
            Console.WriteLine("User Get");
        }

        public override void ProcessPut(Request request)
        {
            Console.WriteLine("User Put");
        }

        public override void ProcessPost(Request request)
        {
            Console.WriteLine("User Post");
        }

        public override void ProcessDelete(Request request)
        {
            Console.WriteLine("User Delete");
        }
    }
}