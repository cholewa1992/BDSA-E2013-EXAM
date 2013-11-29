using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public class FavouriteRequestController : AbstractRequestController
    {
        public override string Keyword { get; set; }

        public FavouriteRequestController()
        {
            Keyword = "Favourite";
        }

        public override void ProcessGet(Request request)
        {
            Console.WriteLine("Favourite Get");
        }

        public override void ProcessPut(Request request)
        {
            Console.WriteLine("Favourite Put");
        }

        public override void ProcessPost(Request request)
        {
            Console.WriteLine("Favourite Post");
        }

        public override void ProcessDelete(Request request)
        {
            Console.WriteLine("Favourite Delete");
        }
    }
}
