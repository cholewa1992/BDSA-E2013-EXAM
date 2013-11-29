using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;

namespace WebServer
{
    public class MovieRequestController : AbstractRequestController
    {
        public MovieRequestController()
        {
            Keyword = "Movie";
        }

        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            Console.WriteLine("Movie Get... id: " + nameValueCollection["id"] + "  Name: " + nameValueCollection["name"]);

            //Console.WriteLine(storage.Get<Movies>(int.Parse(nameValueCollection["id"])).Title);

            return (storage => storage.Get<Movies>(int.Parse(nameValueCollection["id"])));
        }

        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            Console.WriteLine("Movie Put");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            Console.WriteLine("Movie Post");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            Console.WriteLine("Movie Delete");
            return (storage => "Not Yet Implemented");
        }

    }
}
