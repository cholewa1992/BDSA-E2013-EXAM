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
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class FavouriteRequestController : AbstractEntityRequestController
    {
        public FavouriteRequestController()
        {
            Keyword = "Favourite";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //TODO make checks

            //Get the request value of the url
            int id = GetUrlArgument(request.Method);
            
#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Favourites List Get was invoked... " + "id: " + id);
#endif

            //Return the delegate 
            return (storage =>
            {
                FavouriteList favouriteList = storage.Get<FavouriteList>(id);

                string json = JSonParser.Parse(
                    "id", "" + favouriteList.Id,
                    "title", "" + favouriteList.Title,
                    "userAccId", "" + favouriteList.UserAccId
                    );

                return Encoder.Encode(json);
            });
        }

        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Favourites List Post was invoked..."
                + " title: " + values["title"]
                + " userAccId: " + values["userAccId"]
                );

            //(Should we create an byte[] to parse or parse parameters?)
            FavouriteList favouriteList = new FavouriteList()
            {
                Title = values["title"],
                UserAccId = int.Parse(values["userAccId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Add<FavouriteList>(favouriteList);

                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }

        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Favourites List Put was invoked..."
                + " id: " + values["id"] 
                + " title: " + values["title"]
                + " userAccId: " + values["userAccId"]
                );

            //(Should we create an byte[] to parse or parse parameters?)
            FavouriteList favouriteList = new FavouriteList()
            {
                Id = int.Parse(values["id"]),
                Title = values["title"],
                UserAccId = int.Parse(values["userAccId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Update<FavouriteList>(favouriteList);

                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Favourites List Delete was invoked..."
                + " id: " + values["id"]
                );

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<FavouriteList>(int.Parse(values["id"]));

                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }
    }
}
