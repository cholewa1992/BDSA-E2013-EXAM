using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class FavouriteRequestController : AbstractRequestController
    {
        public FavouriteRequestController()
        {
            Keyword = "Favourite";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        public override Func<IStorageConnectionBridgeFacade, object> ProcessGet(Request request)
        {
            Console.WriteLine("Favourite Get");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridgeFacade, object> ProcessPut(Request request)
        {
            Console.WriteLine("Favourite Put");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridgeFacade, object> ProcessPost(Request request)
        {
            Console.WriteLine("Favourite Post");
            return (storage => "Not Yet Implemented");
        }

        public override Func<IStorageConnectionBridgeFacade, object> ProcessDelete(Request request)
        {
            Console.WriteLine("Favourite Delete");
            return (storage => "Not Yet Implemented");
        }
    }
}
