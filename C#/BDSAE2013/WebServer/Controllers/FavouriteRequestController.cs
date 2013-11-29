using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    public class FavouriteRequestController : AbstractRequestController
    {
        public FavouriteRequestController()
        {
            Keyword = "Favourite";
        }

        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            Console.WriteLine("Favourite Get");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            Console.WriteLine("Favourite Put");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            Console.WriteLine("Favourite Post");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            Console.WriteLine("Favourite Delete");
            return (storage => "Not Yet Implemented");
        }
    }
}
