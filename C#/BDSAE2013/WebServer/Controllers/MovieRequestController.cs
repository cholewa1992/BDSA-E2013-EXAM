using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public class MovieRequestController : AbstractRequestController
    {
        public override string Keyword { get; set; }

        public MovieRequestController()
        {
            Keyword = "Movie";
        }

        public override void ProcessGet(Request request)
        {
            Console.WriteLine("Movie Get");
        }

        public override void ProcessPut(Request request)
        {
            Console.WriteLine("Movie Put");
        }

        public override void ProcessPost(Request request)
        {
            Console.WriteLine("Movie Post");
        }

        public override void ProcessDelete(Request request)
        {
            Console.WriteLine("Movie Delete");
        }

    }
}
